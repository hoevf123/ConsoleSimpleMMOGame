using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // 접속 정보
    internal class ConnectionAuthenticationInfo {
        int authKey = 0;    //접속 키(유일 키, 이 키가 일치하지 않으면 비인가 접속으로 간주)
        int ipAddr = 0;     //접속 당시 인터넷 ip 주소.
        int macAddr = 0;    //접속 당시 인터넷의 Mac 주소.
        DateTime establishedDate = DateTime.MinValue;
        public ConnectionAuthenticationInfo(int authKey, int macAddr, DateTime establishedDate) {
            this.authKey = authKey;
            this.macAddr = macAddr;
            this.establishedDate = establishedDate;
        }
    }

    // 접속 인증기
    internal class ConnectionAuthenticator
    {
        private System.Random random = new Random();
        //키: 접속 키, 값: 커넥션 인가 정보.
        public Dictionary<int, ConnectionAuthenticationInfo> connectionPool = new Dictionary<int, ConnectionAuthenticationInfo>();

        public ConnectionAuthenticator() { 
        }

        //새 키 발급
        public int AddConnectionAuthKey()
        {
            bool isKeyCollide = true;
            int toGiveNewAuthKey = 0;
            do
            {
                //중복되지 않은 키가 나올 때까지 새 키 발급.
                //키 발급 범위는 인증 키 납치를 방지하기 위해 무작위 숫자 번호로 발급.
                toGiveNewAuthKey = random.Next(1001, int.MaxValue);
                isKeyCollide = connectionPool.ContainsKey(toGiveNewAuthKey);    // 키 충돌 확인
            } while (isKeyCollide);
            return toGiveNewAuthKey;
        }

        //키 반납
        public bool RemoveConnectionAuthKey(int authKey)
        {
            bool isKeyCollide = connectionPool.ContainsKey(authKey);
            if (isKeyCollide)
            {
                connectionPool.Remove(authKey);
                Console.WriteLine($"authKey ({authKey})이/가 제거되었습니다.");
            }
            return isKeyCollide;
        }
    }
}
