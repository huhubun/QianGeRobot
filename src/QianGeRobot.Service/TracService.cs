using CsvHelper;
using QianGeRobot.Core;
using QianGeRobot.Core.Trac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace QianGeRobot.Services
{
    public class TracService
    {
        private readonly QianGeConfigs _configs;

        public TracService(
            QianGeConfigs configs
            )
        {
            _configs = configs;
        }

        public List<TracQueryResultItem> GetMonthlyTickets(int year, int month)
        {
            if (year < 2019)
            {
                throw new ArgumentException("值必须大于等于 2019（只能查2019年及以后的数据）", nameof(year));
            }

            if (month < 1 || month > 12)
            {
                throw new ArgumentException("值只能在 1 到 12 之间（1~12月）", nameof(month));
            }

            var deadline4devQueryString = $"{year}-{month.ToString().PadLeft(2, '0')}";

            var url = $"{_configs.Trac.Url}/query?owner=bill.nong&owner=aaron.wang&owner=shawn.xia&owner=andy.sui&owner=garen.nan&owner=nick.sui&owner=young.yang&owner=vayne.xi&owner=seal.yang&owner=blake.dong&owner=augus.liu&deadline4dev=%5E{deadline4devQueryString}&format=csv&max=1000&col=id&col=summary&col=deadline4dev&col=owner&col=type&col=status&col=time&col=confirmed&col=deadline&col=hotel&col=reporter&col=startdate&order=summary";
            var request = CreateRequest(url);

            var response = request.GetResponse();

            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            using (var csv = new CsvReader(reader))
            {
                var result = csv.GetRecords<TracQueryResultItem>().ToList();

                return result;
            }
        }

        private X509Certificate2 GetCertificate()
        {
            var certificateConfig = _configs.Trac.Certificate;
            return new X509Certificate2(certificateConfig.Path, certificateConfig.Password);
        }

        private ICredentials GetCredentials()
        {
            var credentialConfig = _configs.Trac.Credential;
            return new NetworkCredential(credentialConfig.Username, credentialConfig.Password);
        }

        private HttpWebRequest CreateRequest(string url)
        {
            var request = WebRequest.CreateHttp(url);

            request.ClientCertificates.Add(GetCertificate());
            request.Credentials = GetCredentials();

            return request;
        }
    }
}
