using System;
using System.Collections.Concurrent;
using Vinva.Gastronomy.Identity.Domain.Entities;
using Vinva.Gastronomy.Identity.Domain.Services;

namespace Vinva.Gastronomy.Identity.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly ConcurrentDictionary<Guid, UserSession> _sessions = new();
        private readonly object _syncRoot = new();

        public UserSession AddOrUpdateSession(UserSession session)
        {
            ArgumentNullException.ThrowIfNull(session);

            lock (_syncRoot)
            {
                foreach (var existingSession in _sessions.Values.Where(a =>
                    a.UserId == session.UserId && a.Id != session.Id))
                {
                    _sessions.TryRemove(existingSession.Id, out _);
                }

                return _sessions.AddOrUpdate(
                    session.Id,
                    session,
                    (_, _) => session);
            }
        }        

        public UserSession GetSession(Guid userId)
        {
            lock (_syncRoot)
            {
                var session = _sessions.Values.FirstOrDefault(a => a.UserId == userId);
                if (session != null)
                    return session;
            }

            throw new KeyNotFoundException($"Session for user '{userId}' was not found.");
        }

        public void RemoveSession(Guid sessionId)
        {
            _sessions.TryRemove(sessionId, out _);
        }
    }
}