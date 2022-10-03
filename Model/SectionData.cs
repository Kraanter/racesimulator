﻿using System;
namespace Model
{
    public class SectionData
    {
        #region Properties

        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }
        public bool IsFull { get; private set; }

        #endregion

        #region Methods
        
        public void AddParticipant(IParticipant participant, LinkedListNode<Section> currentSection, int distance, Random random)
        {
            if (IsFull) return;
            if (Left == null && random.Next(2) == 1 || Right != null)
            {
                Left = participant;
                DistanceLeft = distance;
            }
            else if (Right == null)
            {
                Right = participant;
                DistanceRight = distance;
            }
            IsFull = Left != null && Right != null;
            participant.CurrentSection = currentSection;
        }
        
        public void MoveParticipant(IParticipant participant, int distance)
        {
            if (participant == Left)
            {
                DistanceLeft += distance;
            }
            else if (participant == Right)
            {
                DistanceRight += distance;
            }
            else 
                throw new Exception("Participant not found");
        }
        
        public void RemoveParticipant(IParticipant participant)
        {
            if (Left == participant)
            {
                Left = null;
                DistanceLeft = 0;
            }
            else if (Right == participant)
            {
                Right = null;
                DistanceRight = 0;
            }
            IsFull = Left != null && Right != null;
        }
        
        public int GetParticipantPosition(IParticipant participant)
        {
            if (Left == participant)
                return DistanceLeft;
            else if (Right == participant)
                return DistanceRight;
            else
                throw new Exception("Participant not found");
        }

        #endregion
    }
}

