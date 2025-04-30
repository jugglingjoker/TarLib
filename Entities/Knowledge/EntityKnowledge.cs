using System;
using System.Collections.Generic;

namespace TarLib.Entities.Knowledge {

    public class EntityKnowledge<TKnowledgeEntity, TKnowledgeSubject>
        where TKnowledgeEntity : IKnowledgeEntity {

        private IKnowledgeEntity Keeper { get; }
        private float AwarenessDuration { get; }
        private List<(TKnowledgeSubject subject, float remainingTime)> SubjectBacklog { get; set; } = new List<(TKnowledgeSubject subject, float remainingTime)>();

        public TKnowledgeSubject Subject { get; private set; }

        public EntityKnowledge(IKnowledgeEntity keeper, float awarenessDuration) {
            Keeper = keeper;
            AwarenessDuration = awarenessDuration;
        }

        public void Update(TKnowledgeSubject subject, float elapsedTime, bool applyImmediately = false) {
            if (applyImmediately) {
                Subject = subject;
                SubjectBacklog.Clear();
            } else {
                SubjectBacklog.Add((subject, AwarenessDuration));
                var backlogToRemove = new List<(TKnowledgeSubject subject, float remainingTime)>();
                for (int i = 0; i < SubjectBacklog.Count; i++) {
                    SubjectBacklog[i] = (SubjectBacklog[i].subject, SubjectBacklog[i].remainingTime - elapsedTime);
                    if (SubjectBacklog[i].remainingTime < 0) {
                        Subject = SubjectBacklog[i].subject;
                        backlogToRemove.Add(SubjectBacklog[i]);
                    }
                }
                backlogToRemove.ForEach(backlog => SubjectBacklog.Remove(backlog));
            }
        }

        public override bool Equals(object obj) {
            return obj is EntityKnowledge<TKnowledgeEntity, TKnowledgeSubject> knowledge &&
                   EqualityComparer<TKnowledgeSubject>.Default.Equals(Subject, knowledge.Subject);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Subject);
        }

        public static bool operator ==(EntityKnowledge<TKnowledgeEntity, TKnowledgeSubject> knowledge, TKnowledgeSubject subject) {
            return knowledge.Subject.Equals(subject);
        }

        public static bool operator !=(EntityKnowledge<TKnowledgeEntity, TKnowledgeSubject> knowledge, TKnowledgeSubject subject) {
            return !knowledge.Subject.Equals(subject);
        }

        public static implicit operator TKnowledgeSubject(EntityKnowledge<TKnowledgeEntity, TKnowledgeSubject> knowledge) {
            return knowledge.Subject;
        }
    }
}
