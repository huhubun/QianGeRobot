using System.Collections.Generic;

namespace QianGeRobot.Core
{
    public class QianGeConfigs
    {
        public DingTalkConfig DingTalk { get; set; }

        public TracConfig Trac { get; set; }

        public List<PeopleConfig> Peoples { get; set; }
    }

    public class DingTalkConfig
    {
        public string RobotUrl { get; set; }
    }

    public class TracConfig
    {
        public string Url { get; set; }

        public TracCertificateConfig Certificate { get; set; }

        public TracCredentialConfig Credential { get; set; }
    }

    public class TracCertificateConfig
    {
        public string Path { get; set; }

        public string Password { get; set; }
    }

    public class TracCredentialConfig
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class PeopleConfig
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }
    }
}