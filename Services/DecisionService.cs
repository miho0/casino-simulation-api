using CasinoSimulationApi.Models;
using System.Reflection.Metadata.Ecma335;

namespace CasinoSimulationApi.Data
{
    // for now we assume DAS is not offered
    // IDEA: ace mode
    public class DecisionService
    {
        public Dictionary<string, Decision> DecisionDictionary { get; set; }
        public Dictionary<string, bool> ShouldSplitDictionary { get; set; }

        public DecisionService()
        {
            DecisionDictionary = new Dictionary<string, Decision>
            {
                { "9-2", Decision.Hit },
                { "9-3", Decision.Double },
                { "9-4", Decision.Double },
                { "9-5", Decision.Double },
                { "9-6", Decision.Double },
                { "12-2", Decision.Hit },
                { "12-3", Decision.Hit },
                { "12-4", Decision.Stand },
                { "12-5", Decision.Stand },
                { "12-6", Decision.Stand },
                { "7-2s", Decision.Double },
                { "7-3s", Decision.Double },
                { "7-4s", Decision.Double },
                { "7-5s", Decision.Double },
                { "7-6s", Decision.Double },
                { "7-7s", Decision.Stand },
                { "7-8s", Decision.Stand },
                { "7-9s", Decision.Hit },
                { "7-10s", Decision.Hit },
                { "7-1s", Decision.Hit },
                { "6-2s", Decision.Hit },
                { "6-3s", Decision.Double },
                { "6-4s", Decision.Double },
                { "6-5s", Decision.Double },
                { "6-6s", Decision.Double },
                { "5-2s", Decision.Hit },
                { "5-3s", Decision.Hit },
                { "5-4s", Decision.Stand },
                { "5-5s", Decision.Stand },
                { "5-6s", Decision.Stand },
                { "4-2s", Decision.Hit },
                { "4-3s", Decision.Hit },
                { "4-4s", Decision.Double },
                { "4-5s", Decision.Double },
                { "4-6s", Decision.Double },
                { "3-2s", Decision.Hit },
                { "3-3s", Decision.Hit },
                { "3-4s", Decision.Hit },
                { "3-5s", Decision.Stand },
                { "3-6s", Decision.Stand },
                { "2-2s", Decision.Hit },
                { "2-3s", Decision.Hit },
                { "2-4s", Decision.Hit },
                { "2-5s", Decision.Double },
                { "2-6s", Decision.Double },
            };

            ShouldSplitDictionary = new Dictionary<string, bool>
            {
                { "18-7", false },
                { "18-8", true },
                { "18-9", true },
                { "18-10", false },
                { "18-1", false },
                { "14-7", true },
                { "12-2", false },
                { "12-3", true },
                { "12-4", true },
                { "12-5", true },
                { "12-6", true },
                { "12-7", false },
                { "6-2", false },
                { "6-3", false },
                { "6-4", true },
                { "6-5", true },
                { "6-6", true },
                { "6-7", true },
                { "4-2", false },
                { "4-3", false },
                { "4-4", true },
                { "4-5", true },
                { "4-6", true },
                { "4-7", true },

            };
        }

        // string format: "PlayerTotal-DealerFaceUpCard"
        public Decision GetDecisionFromDictionary(int playerTotal, int dealerFaceUpCard, bool soft = false)
        {
            string key = $"{playerTotal}-{dealerFaceUpCard}";
            if (soft)
            {
                key += "s";
            }
            return DecisionDictionary[key];
        }

        public bool GetShouldSplitFromDictionary(int playerTotal, int dealerFaceUpCard)
        {
            string key = $"{playerTotal}-{dealerFaceUpCard}";
            return ShouldSplitDictionary[key];
        }

        public Decision Decide(BlackjackGame game)
        {
            if (game.IsPairPlayer)
            {
                if(ShouldSplit(game)) return Decision.Split;
                else return DecideRegular(game);
            }
            if (game.SoftTotalPlayer == true)
            {
                return DecideSoft(game);
            } else
            {
                return DecideRegular(game);
            }
        }

        public Decision DecideRegular(BlackjackGame game)
        {
            int playerTotal = game.PlayerCards.Sum();
            int dealerFaceUpCard = game.DealerFaceUpCard;
            if (dealerFaceUpCard == 1)
            {
                dealerFaceUpCard = 11;
            }

            if (playerTotal < 9)
            {
                return Decision.Hit;
            }

            if (playerTotal > 16)
            {
                return Decision.Stand;
            }

            if (playerTotal > 12)
            {
                if (dealerFaceUpCard < 7)
                {
                    return Decision.Stand;
                }
                else
                {
                    return Decision.Hit;
                }
            }

            if (playerTotal == 12)
            {
                if (dealerFaceUpCard > 6)
                {
                    return Decision.Hit;
                }
                else
                {
                    return GetDecisionFromDictionary(playerTotal, dealerFaceUpCard);
                }
            }

            if (playerTotal == 11)
            {
                return Decision.Double;
            }

            if (playerTotal == 10)
            {
                if (dealerFaceUpCard < 10)
                {
                    return Decision.Double;
                }
                else
                {
                    return Decision.Hit;
                }
            }

            if (playerTotal == 9)
            {
                if (dealerFaceUpCard > 6)
                {
                    return Decision.Hit;
                }
                else
                {
                    return GetDecisionFromDictionary(playerTotal, dealerFaceUpCard);
                }
            }
            return Decision.Error;
        }

        public Decision DecideSoft(BlackjackGame game)
        {
            int playerTotalWithoutAce = game.CalculateTotalPlayerWithoutAce();
            int dealerFaceUpCard = game.DealerFaceUpCard;

            if (playerTotalWithoutAce == 1)
            {
                if (dealerFaceUpCard > 6)
                {
                    return Decision.Hit;
                }
                else
                {
                    return GetDecisionFromDictionary(playerTotalWithoutAce, dealerFaceUpCard);
                }
            }

            if (playerTotalWithoutAce > 7) 
            {
                return Decision.Stand;
            }

            if (playerTotalWithoutAce < 7 && dealerFaceUpCard > 6)
            {
                return Decision.Hit;
            }

            return GetDecisionFromDictionary(playerTotalWithoutAce, dealerFaceUpCard, true);
        }

        public bool ShouldSplit(BlackjackGame game)
        {
            int card = game.PlayerCards[0];
            int dealerFaceUpCard = game.DealerFaceUpCard;
            if (card == 1 || card == 8)
            {
                return true;
            }

            if (card == 10 || card == 5 || card == 4)
            {
                return false;
            }

            if (card > 6 && dealerFaceUpCard < 7)
            {
                return true;
            }

            if (card < 8 && dealerFaceUpCard > 7)
            {
                return false;
            }

            return GetShouldSplitFromDictionary(card*2, dealerFaceUpCard);
        }
    }
}
