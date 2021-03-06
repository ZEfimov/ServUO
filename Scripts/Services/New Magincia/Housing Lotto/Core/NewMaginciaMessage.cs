using System;

namespace Server.Engines.NewMagincia
{
    public class NewMaginciaMessage
    {
        public static readonly TimeSpan DefaultExpirePeriod = TimeSpan.FromDays(7);

        private readonly TextDefinition m_Title;
        private readonly TextDefinition m_Body;
        private readonly string m_Args;
        private readonly DateTime m_Expires;

        public TextDefinition Title => m_Title;
        public TextDefinition Body => m_Body;
        public string Args => m_Args;
        public DateTime Expires => m_Expires;

        public bool Expired => m_Expires < DateTime.UtcNow;

        public NewMaginciaMessage(TextDefinition title, TextDefinition body)
            : this(title, body, DefaultExpirePeriod, null)
        {
        }

        public NewMaginciaMessage(TextDefinition title, TextDefinition body, string args)
            : this(title, body, DefaultExpirePeriod, args)
        {
        }

        public NewMaginciaMessage(TextDefinition title, TextDefinition body, TimeSpan expires)
            : this(title, body, expires, null)
        {
        }

        public NewMaginciaMessage(TextDefinition title, TextDefinition body, TimeSpan expires, string args)
        {
            m_Title = title;
            m_Body = body;
            m_Args = args;
            m_Expires = DateTime.UtcNow + expires;
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            TextDefinition.Serialize(writer, m_Title);
            TextDefinition.Serialize(writer, m_Body);
            writer.Write(m_Expires);
            writer.Write(m_Args);
        }

        public NewMaginciaMessage(GenericReader reader)
        {
            int v = reader.ReadInt();

            m_Title = TextDefinition.Deserialize(reader);
            m_Body = TextDefinition.Deserialize(reader);
            m_Expires = reader.ReadDateTime();
            m_Args = reader.ReadString();
        }
    }
}