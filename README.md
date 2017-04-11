Meerkat Name Parser
===================

The [Meerkat.NameParser](https://www.nuget.org/packages/Meerkat.NameParser/) library allows you to parse personal names into their constitutent parts.

This is most useful when you've been given a list of full names e.g. "Mr Bob Smith BSc" that you then need to store into a domain model and/or use for other purposes such as personalising an email or a website, addressing a letter etc.

[![NuGet](https://img.shields.io/nuget/v/Meerkat.NameParser.svg)](https://www.nuget.org/packages/Meerkat.NameParser/)
[![Build status](https://ci.appveyor.com/api/projects/status/7ycnghu7s0umys9e/branch/master?svg=true)](https://ci.appveyor.com/project/PaulHatcher/meerkat-nameparser/branch/master)

Welcome to contributions from anyone.

You can see the version history [here](RELEASE_NOTES.md).

## Build the project
* Windows: Run *build.cmd*

I have my tools in C:\Tools so I use *build.cmd Default tools=C:\Tools encoding=UTF-8*

## Library License

The library is available under the [MIT License](http://en.wikipedia.org/wiki/MIT_License), for more information see the [License file][1] in the GitHub repository.

 [1]: https://github.com/phatcher/Meerkat.NameParser/blob/master/License.md

## Getting Started

## Name Structure
The library defines an interface which exposes standardized parts of a name, all of which are optional to allow for flexible handling and also poor data quality:

* T)itle - Typically "Mr", "Mrs", "Ms" but we can also recognise professional ("Dr", "Prof" etc) and military ("Captain", "Major", "Lt" etc) titles
* G)iven name(s) - The personal names e.g. "Bob", "Jean-Paul"
* P)refix - Prefix before the family name e.g. "van", "de", used as we might want/need to collate without the prefix for searching/sorting.
* F)amily name - The family name e.g. "Smith", "Gonzalez-Byaz"
* S)uffix - Part of the family name typically generational e.g. "Jnr", "II", "III", against might not be needed for collate/search
* L)etters - Can be civil awards e.g. "OBE", "CBE", military awards e.g. "VC", "GC", "DSM", "DSO", degrees e.g. "BSc", "PhD", professional awards e.g. "CEng" or membership of professional societies e.g. "FRCS", "FBCS", "MACM" etc

We use given and family name in the library rather than forename/surname to avoid confusion with non-Western names where the display order is "Family Given" rather than "Given Family"

We also have two other properties
* Salutation - For the most part this is simple e.g. "Dear Given" or "Dear Title Surname" depending on how formal you want to be, however in British English you have anomalies such as "Sir George Bingham" is "Dear Sir George" and the "Bishop Of Cantebury" is "My Lord Bishop"
* Envelope - The name you would put on the envelope e.g. "Mr Bob Smith"

These are controlled by SalutationFormat and EnvelopeFormat so that they may be generated from the name parts or set explicitly.

## Parsing Methodology
The approach is to split the name into tokens and then to identify the various name parts present using heuristics or recogising explicit tokens. This is probably best explained with a few examples.

* Bob Smith -> (GF) : Using the heuristic that without any other information the last token is the Family name and others are Given names
* Bob George Smith -> (GGF) : Same as above
* Bob Smith-Johnson -> (GF) : The hypen binds to two family name parts together, then the GF heuristic applies
* Jean-Paul Gautier -> (GF) : First heuristic except the hypen binding working for given names, BTW we ignore whitespace around the hypen so "Jean - Paul Gautier" etc would parse the same way.

When we analyse the input string we create Symbols which implement IToken

    public interface IToken
    {
        /// <summary>
        /// Class of the symbol, application dependent
        /// </summary>
        TokenClass Token { get; set; }

        /// <summary>
        /// Value of token, e.g. the keyword/variable name/constant
        /// </summary>
        object Value { get; set; }
    }

For titles, suffixes and letters we have explcit keyword lists that can be expanded upon if your data set has values not already handled. These are all stored as NameSymbol entities

    public class NameSymbol : Symbol
    {
        public NameSymbol()
        {
            Token = TokenClass.Value;
            Properties = new Dictionary<string, object>();
        }

        public NameToken NameType { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }

Rationale of the additional property bag is to allow other information to stored against the NameSymbol which can be used to derive more information about the person or how they are displayed e.g.

    { 
        "NameType": "Title", 
        "Value": "Bishop", 
        "Properties":{ 
            "Gender": "M", 
            "SalutationFormat": "My Lord Bishop" 
        } 
    },
    { 
        "NameType": "Title", 
        "Value": "Sir", 
        "Properties":{ 
            "Gender": "M", 
            "SalutationFormat": "DG" 
        },
    }

This is only limited by what you can infer from the data and what you need for your application, here's some examples from the Letter entries:-

    { "NameType": "Academic", "Value": "BEcon", "Properties":{ "Degree": "Bachelors", "Subject": "Economnics" } },
    { "NameType": "Academic", "Value": "BEd", "Properties":{ "Degree": "Bachelors", "Subject": "Education" } },
    { "NameType": "Academic", "Value": "BEng", "Properties":{ "Degree": "Bachelors", "Subject": "Engineering" } },
    { "NameType": "Academic", "Value": "BPharm", "Properties":{ "Degree": "Bachelors", "Subject": "Pharmacology" } },



