using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal enum UserRole
    {
        Guest,
        User,
        Moderator,
        Admin,
        LocalAdmin
    }

    internal class ConnectedUser
    {
        public string Name { get; private set; }
        public UserRole Role { get; private set; }

        private readonly ConnectionAuthenticationInfo connectionAuthenticationInfo;
        private readonly List<ICommand> availableCommands = new List<ICommand>();

        public int AuthKey
        {
            get { return connectionAuthenticationInfo.AuthKey; }
        }

        public DateTime ConnectedAtUtc
        {
            get { return connectionAuthenticationInfo.EstablishedDate; }
        }

        public ConnectedUser(string name, ConnectionAuthenticationInfo connectionAuthenticationInfo)
        {
            if (connectionAuthenticationInfo == null) throw new ArgumentNullException("connectionAuthenticationInfo");

            Name = string.IsNullOrWhiteSpace(name) ? "UnknownUser" : name;
            this.connectionAuthenticationInfo = connectionAuthenticationInfo;

            // 로컬 접속은 기본적으로 권한을 높게 부여
            Role = connectionAuthenticationInfo.IsLocalUser ? UserRole.LocalAdmin : UserRole.User;
        }

        public bool IsAuthenticatedBy(ConnectionAuthenticator authenticator)
        {
            if (authenticator == null) return false;
            return authenticator.IsAuthorized(
                connectionAuthenticationInfo.AuthKey,
                connectionAuthenticationInfo.IpAddress,
                connectionAuthenticationInfo.MacAddress);
        }

        public void SetRole(UserRole role)
        {
            Role = role;
        }

        public bool HasPermission(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission)) return false;

            switch (Role)
            {
                case UserRole.LocalAdmin:
                case UserRole.Admin:
                    return true;
                case UserRole.Moderator:
                    return permission != "system.shutdown";
                case UserRole.User:
                    return permission == "world.read" || permission == "world.join";
                case UserRole.Guest:
                default:
                    return permission == "world.read";
            }
        }

        public void AddAvailableCommand(ICommand command)
        {
            if (command == null) return;
            if (availableCommands.Contains(command)) return;
            availableCommands.Add(command);
        }

        public bool RemoveAvailableCommand(ICommand command)
        {
            if (command == null) return false;
            return availableCommands.Remove(command);
        }

        public void ExecuteAvailableCommands()
        {
            foreach (ICommand command in availableCommands)
            {
                command.Execute();
            }
        }
    }
}
