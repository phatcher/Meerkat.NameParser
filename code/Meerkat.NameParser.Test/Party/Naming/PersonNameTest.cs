using Meerkat.Party.Naming;
using NUnit.Framework;

namespace Meerkat.Test.Party.Naming
{
    [TestFixture]
    public class PersonNameTest
    {
        [Test]
        public void Title()
        {
            ParseName("Mr", envelope: "Mr", title: "Mr");
        }

        [Test]
        public void MultipleTitle()
        {
            ParseName("The Rt Hon", envelope: "The Rt Hon", title: "The Rt Hon");
        }

        [Test]
        public void Suffix()
        {
            ParseName("Jnr", envelope: "Jnr", suffix: "Jnr");
        }

        [Test]
        public void MultipleSuffix()
        {
            ParseName("Jnr II", envelope: "Jnr II", suffix: "Jnr II");
        }

        [Test]
        public void ForenameSurname()
        {
            ParseName("Paul Smith", string.Empty, "Paul Smith", string.Empty, "Paul", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void TitleSurname()
        {
            ParseName("Mr Smith", string.Empty, "Mr Smith", "Mr", string.Empty, string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void TitleForenameSurname()
        {
            ParseName("Mr Paul Smith", string.Empty, "Mr Paul Smith", "Mr", "Paul", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void TwoForenameSurname()
        {
            ParseName("John Paul Smith", string.Empty, "John Paul Smith", string.Empty, "John Paul", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void SurnameForename()
        {
            ParseName("Smith Paul", "STF", "Paul Smith", string.Empty, "Paul", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void SurnameTitle()
        {
            ParseName("Smith Mr", "STF", "Mr Smith", "Mr", string.Empty, string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void SurnameTitleForename()
        {
            ParseName("Smith Mr Paul", "STF", "Mr Paul Smith", "Mr", "Paul", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void SurnameTitleInitials()
        {
            ParseName("Smith Mr J D", "STF", "Mr J D Smith", "Mr", "J D", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void SurnameSuffixTitleInitials()
        {
            ParseName("Smith III Mr J D", "STF", "Mr J D Smith III", "Mr", "J D", string.Empty, "Smith", "III");
        }

        [Test]
        public void DoubleSurnameTitle()
        {
            ParseName("Hatcher Sanchez Mr John", "STF", "Mr John Hatcher Sanchez", "Mr", "John", string.Empty, "Hatcher Sanchez");
        }

        [Test]
        public void DoubleRecognisedSurnameTitle()
        {
            ParseName("Gomez Sanchez Mr John", "STF", "Mr John Gomez Sanchez", "Mr", "John", string.Empty, "Gomez Sanchez");
        }

        [Test]
        public void HyphenatedSurnameTitle()
        {
            ParseName("Bavington-Smith Lady", "STF", "Lady Bavington-Smith", "Lady", string.Empty, string.Empty, "Bavington-Smith", string.Empty);
        }

        [Test]
        public void HyphenatedSurnameForename()
        {
            ParseName("Bavington-Smith John", "STF", "John Bavington-Smith", string.Empty, "John", string.Empty, "Bavington-Smith", string.Empty);
        }

        [Test]
        public void HyphenatedSurnameTitleForename()
        {
            ParseName("Bavington-Smith Lady Penelope", "STF", "Lady Penelope Bavington-Smith", "Lady", "Penelope", string.Empty, "Bavington-Smith", string.Empty);
        }

        [Test]
        public void HyphenatedForenameSurname()
        {
            ParseName("Jean-Paul Smith", string.Empty, "Jean-Paul Smith", string.Empty, "Jean-Paul", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void ForenameHyphenatedSurname()
        {
            ParseName("Paul Smith-Jones", string.Empty, "Paul Smith-Jones", string.Empty, "Paul", string.Empty, "Smith-Jones", string.Empty);
        }

        [Test]
        public void TitleHyphenatedForenameSurname()
        {
            ParseName("Mr Jean-Paul Smith", string.Empty, "Mr Jean-Paul Smith", "Mr", "Jean-Paul", string.Empty, "Smith", string.Empty);
        }

        [Test]
        public void TitleForenameHyphenatedSurname()
        {
            ParseName("Mr Paul Smith-Jones", string.Empty, "Mr Paul Smith-Jones", "Mr", "Paul", string.Empty, "Smith-Jones", string.Empty);
        }

        [Test]
        public void ForenameDoubleHyphenatedSurname()
        {
            ParseName("Paul Smith-Jones-Watt", string.Empty, "Paul Smith-Jones-Watt", string.Empty, "Paul", string.Empty, "Smith-Jones-Watt", string.Empty);
        }

        [Test]
        public void TitleTripleSurname()
        {
            ParseName("The Lord Smith-Jones-Watt", envelope: "The Lord Smith-Jones-Watt", title: "The Lord", surname: "Smith-Jones-Watt");
        }

        [Test]
        public void TitleForenameTripleSurname()
        {
            ParseName("The Lord Paul Smith-Jones-Watt", envelope: "The Lord Paul Smith-Jones-Watt", title: "The Lord", forename: "Paul", surname: "Smith-Jones-Watt");
        }

        [Test]
        public void TitleSurnameSuffix()
        {
            ParseName("Mr Smith II", envelope: "Mr Smith II", title: "Mr", surname: "Smith", suffix: "II");
        }

        [Test]
        public void ForenameSurnameSuffix()
        {
            ParseName("Paul Smith II", envelope: "Paul Smith II", forename: "Paul", surname: "Smith", suffix: "II");
        }

        [Test]
        public void TitleForenameSurnameSuffix()
        {
            ParseName("Dr Paul Smith Jnr", envelope: "Dr Paul Smith Jnr", title: "Dr", forename: "Paul", surname: "Smith", suffix: "Jnr");
        }

        [Test]
        public void TitleForenameSurnameLetters()
        {
            ParseName("Dr Paul Smith BSc", envelope: "Dr Paul Smith", title: "Dr", forename: "Paul", surname: "Smith");
        }

        [Test]
        public void TitleForenameSurnameSuffixLetters()
        {
            ParseName("Mr Jean-Paul Smith-Jones II MA PC", envelope: "Mr Jean-Paul Smith-Jones II", title: "Mr", forename: "Jean-Paul", surname: "Smith-Jones", suffix: "II");
        }

        [Test]
        public void MultiTitleForenameSurnameMultiSuffix()
        {
            ParseName("Mw Drs Ir J da Puten III Jnr", envelope: "Mw Drs Ir J da Puten III Jnr", title: "Mw Drs Ir", forename: "J", surname: "da Puten", suffix: "III Jnr");
        }

        [Test]
        public void Nasty()
        {
            ParseName("The Rt Hon The Lord The admiral of the fleet Peter Edward Walker of Worcester II MBE PC", title: "The Rt Hon The Lord The Admiral of The fleet", forename: "Peter Edward", surname: "Walker of Worcester", suffix: "II");
        }

        [Test]
        public void SpanishSurname()
        {
            ParseName("Roman Gonzalez y Garcia", envelope: "Roman Gonzalez y Garcia", forename: "Roman", surname: "Gonzalez y Garcia");
        }

        [Test]
        public void Test10()
        {
            ParseName("W. James McNerney Jr SC", envelope: "W James McNerney Jr", forename: "W James", surname: "McNerney", suffix: "Jr");
        }

        [Test]
        public void ForenamesSurnameLettrs()
        {
            ParseName("Alexander Frank Masters CEng FIMechE FIEE ", envelope: "Alexander Frank Masters", forename: "Alexander Frank", surname: "Masters");
        }

        [Test]
        public void ForenameMultiSurname()
        {
            ParseName("German Gonzalez del Valle y Chavarri", forename: "German", surname: "Gonzalez del Valle y Chavarri");
        }

        [Test]
        public void Test13()
        {
            ParseName("Peter Anthony Green BSc ( Econ ) ", envelope: "Peter Anthony Green", forename: "Peter Anthony", surname: "Green");
        }

        [Test]
        public void Test14()
        {
            ParseName("J F Power MA ( Oxon ) FCA FCMA ", envelope: "J F Power", forename: "J F", surname: "Power");
        }

        [Test]
        public void GermanTitleLettersForenameSurname()
        {
            ParseName("Dipl.-Kfm. Dr.rer.pol. Heyo Schmiedeknecht", envelope: "Dipl-Kfm Dr Rer Pol Heyo Schmiedeknecht", title: "Dipl-Kfm Dr Rer Pol", forename: "Heyo", surname: "Schmiedeknecht");
        }

        [Test]
        public void ApostrophedSurname()
        {
            ParseName("John D'Arcy", envelope: "John D'Arcy", forename: "John", surname: "D'Arcy");
            ParseName("John D`Arcy", envelope: "John D`Arcy", forename: "John", surname: "D`Arcy");
        }

        [Test]
        public void ReverseApostrophedSurname()
        {
            ParseName("D'Arcy John", "STF", envelope: "John D'Arcy", forename: "John", surname: "D'Arcy");
        }

        [Test]
        public void NonRecognishedTitlePresent()
        {
            ParseName("XXX Colonel John Smith", envelope: "XXX Colonel John Smith", title: "XXX Colonel", forename: "John", surname: "Smith");
        }

        [Test]
        public void ForenameDoubleTitle()
        {
            ParseName("Buffy the Vampire Slayer", envelope: "Buffy The Vampire Slayer", title: "The Vampire Slayer", forename: "Buffy");
        }

        [Test]
        public void ForenameTitle()
        {
            ParseName("Kermit The Frog", envelope: "Kermit The Frog", title: "The Frog", forename: "Kermit");
        }

        [Test]
        public void ForenameTitleII()
        {
            ParseName("Richard The Lionheart", envelope: "Richard The Lionheart", title: "The Lionheart", forename: "Richard");
        }

        private void ParseName(string value, string format = "", string envelope = "", string title = "", string forename = "", string prefix = "", string surname = "", string suffix = "")
        {
            var parser = format == "STF" ? ParserFactory.StandardPersonParser(true) : ParserFactory.StandardPersonParser();

            // Assign the parser and the value
            var name = new PersonName
            {
                Parser = parser
            };
            name.Parse(value, format);

            // Check the result
            if (envelope.Length > 0)
            {
                Assert.That(name.Envelope, Is.EqualTo(envelope), name + " Envelope");
            }
            Assert.That(name.Title, Is.EqualTo(title), name + " Title");
            Assert.That(name.Given, Is.EqualTo(forename), name + " Forename");
            Assert.That(name.Family.Prefix, Is.EqualTo(prefix), name + " Prefix");
            Assert.That(name.Family.Core, Is.EqualTo(surname), name + " Surname");
            Assert.That(name.Family.Suffix, Is.EqualTo(suffix), name + " Suffix");
        }
    }
}