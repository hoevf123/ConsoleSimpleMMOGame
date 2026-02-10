using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    // 접속 정보
    internal class ConnectionAuthenticationInfo
    {
        public int AuthKey { get; private set; }        // 접속 키(유일 키)
        public string IpAddress { get; private set; }   // 접속 당시 IP 주소
        public string MacAddress { get; private set; }  // 접속 당시 MAC 주소
        public DateTime EstablishedDate { get; private set; }
        public bool IsLocalUser { get; private set; }

        public ConnectionAuthenticationInfo(int authKey, string ipAddress, string macAddress, DateTime establishedDate, bool isLocalUser)
        {
            AuthKey = authKey;
            IpAddress = string.IsNullOrWhiteSpace(ipAddress) ? "127.0.0.1" : ipAddress;
            MacAddress = string.IsNullOrWhiteSpace(macAddress) ? "LOCAL" : macAddress;
            EstablishedDate = establishedDate;
            IsLocalUser = isLocalUser;
        }

        public bool IsSameEndpoint(string ipAddress, string macAddress)
        {
            string normalizedIp = string.IsNullOrWhiteSpace(ipAddress) ? "127.0.0.1" : ipAddress;
            string normalizedMac = string.IsNullOrWhiteSpace(macAddress) ? "LOCAL" : macAddress;
            return IpAddress == normalizedIp && MacAddress == normalizedMac;
        }
    }

    // 접속 인증기
    internal class ConnectionAuthenticator
    {
        private readonly Random random = new Random();

        // 키: 접속 키, 값: 커넥션 인가 정보
        private readonly Dictionary<int, ConnectionAuthenticationInfo> connectionPool = new Dictionary<int, ConnectionAuthenticationInfo>();

        public int CurrentConnectionCount
        {
            get { return connectionPool.Count; }
        }

        public ConnectionAuthenticator()
        {
        }

        // 새 키 생성(등록은 하지 않음)
        private int GenerateConnectionAuthKey()
        {
            bool isKeyCollide;
            int toGiveNewAuthKey;
            do
            {
                // 키 발급 범위는 인증 키 납치를 방지하기 위해 무작위 숫자 번호로 발급
                toGiveNewAuthKey = random.Next(1001, int.MaxValue);
                isKeyCollide = connectionPool.ContainsKey(toGiveNewAuthKey);
            } while (isKeyCollide);

            return toGiveNewAuthKey;
        }

        // 접속 등록
        public ConnectionAuthenticationInfo Connect(string ipAddress, string macAddress, bool isLocalUser)
        {
            int authKey = GenerateConnectionAuthKey();
            ConnectionAuthenticationInfo info = new ConnectionAuthenticationInfo(
                authKey,
                ipAddress,
                macAddress,
                DateTime.UtcNow,
                isLocalUser);

            connectionPool[authKey] = info;
            Console.WriteLine($"authKey ({authKey})가 발급되었습니다. endpoint={info.IpAddress}/{info.MacAddress}, local={info.IsLocalUser}");
            return info;
        }

        // authKey/endpoint 기반 인증
        public bool IsAuthorized(int authKey, string ipAddress, string macAddress)
        {
            ConnectionAuthenticationInfo info;
            if (!connectionPool.TryGetValue(authKey, out info)) return false;
            return info.IsSameEndpoint(ipAddress, macAddress);
        }

        public bool TryGetConnectionInfo(int authKey, out ConnectionAuthenticationInfo info)
        {
            return connectionPool.TryGetValue(authKey, out info);
        }

        // authKey 반납
        public bool RemoveConnectionAuthKey(int authKey)
        {
            bool exists = connectionPool.ContainsKey(authKey);
            if (exists)
            {
                connectionPool.Remove(authKey);
                Console.WriteLine($"authKey ({authKey})이/가 제거되었습니다.");
            }
            return exists;
        }
    }
}
