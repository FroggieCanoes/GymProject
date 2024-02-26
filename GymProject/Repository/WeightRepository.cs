using GymProject.Domain;
using GymProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace GymProject.Repository
{
    public class WeightRepository
    {
        #region constDB
        private string dbPath = String.Empty;
        private string connectionString = String.Empty;

        private const string queryGetWeightById = "SELECT * FROM Weight WHERE WeightID = @WeightID";
        private const string queryGetLatestWeightsForDifferentExercise =
            @"SELECT w.UserID, w.Exercise, w.Weight, w.Day
            FROM Weight w
            INNER JOIN (
                SELECT UserID, Exercise, MAX(Day) AS LastDay
                FROM Weight
                GROUP BY UserID, Exercise
            ) last_weight ON w.UserID = last_weight.UserID
                        AND w.Exercise = last_weight.Exercise
                        AND w.Day = last_weight.LastDay;";
        private const string queryGetAverageWeightByExercise = "SELECT Exercise, AVG(Weight) AS AverageWeight FROM Weight WHERE UserID = @UserID GROUP BY Exercise";
        private const string queryInsertWeight = "INSERT INTO Weight (Weight, Day, Exercise,UserID) VALUES (@Weight, @Day, @Exercise,@UserID)";
        private const string queryInsertWeights = "INSERT INTO Weight (Weight, Day, Exercise, UserID) VALUES (@Weight, @Day, @Exercise,@UserID)";
        private const string queryUpdateWeightById = "UPDATE Weight SET Weight = @Weight, Day = @Day, Exercise = @Exercise WHERE WeightID = @WeightID";
        private const string queryDeleteWeightById = "DELETE FROM Weight WHERE WeightID = @WeightID";

        #endregion
        public WeightRepository()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            dbPath = Path.Combine(currentDirectory, "DataBase", "GymProjectDB.db");
            connectionString = $"Data Source={dbPath};Version=3;";
        }

        #region Gets
        public Weight GetWeightById(int weightId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(queryGetWeightById, connection))
                {
                    command.Parameters.AddWithValue("@WeightID", weightId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Weight
                            {
                                Kg = Convert.ToDouble(reader["Weight"]),
                                Day = Convert.ToDateTime(reader["Day"]),
                                Exercicie = (int)ParseEnum<ExerciciesEnum>(reader["Exercise"].ToString())

                            };
                        }
                        else
                        {
                            return null; // Si no se encuentra el peso con ese ID
                        }
                    }
                }
            }
        }

        public List<Weight> GetLatestWeightsForDifferentExercise(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(queryGetLatestWeightsForDifferentExercise, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        List<Weight> weights = new List<Weight>();
                        while (reader.Read())
                        {
                            weights.Add(new Weight
                            {
                                Kg = Convert.ToDouble(reader["Weight"]),
                                Day = Convert.ToDateTime(reader["Day"]),
                                Exercicie = (int)ParseEnum<ExerciciesEnum>(reader["Exercise"].ToString())
                            });
                        }
                        Console.WriteLine(connectionString);
                        return weights;
                    }
                }
            }
        }
        public List<Weight> GetAverageWeightByExercise(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(queryGetAverageWeightByExercise, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        List<Weight> weights = new List<Weight>();
                        while (reader.Read())
                        {
                            weights.Add(new Weight
                            {
                                Exercicie = (int)ParseEnum<ExerciciesEnum>(reader["Exercise"].ToString()),
                                Kg = Convert.ToDouble(reader["AverageWeight"])
                            });
                        }
                        return weights;
                    }
                }
            }
        }


        #endregion

        #region Inserts
        public void InsertWeight(Weight newWeight, int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(queryInsertWeight, connection))
                {
                    command.Parameters.AddWithValue("@Weight", newWeight.Kg);
                    command.Parameters.AddWithValue("@Day", DateTime.Now);
                    command.Parameters.AddWithValue("@Exercise", newWeight.Exercicie.ToString());
                    command.Parameters.AddWithValue("@UserID", userId.ToString());

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertWeights(List<Weight> newWeights, int userId)
        {
            if (newWeights == null || newWeights.Count == 0)
            {
                return; // No hay pesos para insertar
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO Weight (Weight, Day, Exercise, UserID) VALUES ");

                for (int i = 0; i < newWeights.Count; i++)
                {
                    queryBuilder.Append($"(@Weight{i}, @Day{i}, @Exercise{i}, @UserID{i})");

                    if (i < newWeights.Count - 1)
                    {
                        queryBuilder.Append(", ");
                    }
                }

                using (var command = new SQLiteCommand(queryBuilder.ToString(), connection))
                {
                    for (int i = 0; i < newWeights.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@Weight{i}", newWeights[i].Kg);
                        command.Parameters.AddWithValue($"@Day{i}", DateTime.Now);
                        command.Parameters.AddWithValue($"@Exercise{i}", newWeights[i].Exercicie.ToString());
                        command.Parameters.AddWithValue($"@UserID{i}", userId.ToString());
                    }

                    command.ExecuteNonQuery();
                }
            }
        }


        #endregion

        #region Updates
        public void UpdateWeightById(int weightId, Weight updatedWeight)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(queryUpdateWeightById, connection))
                {
                    command.Parameters.AddWithValue("@WeightID", weightId);
                    command.Parameters.AddWithValue("@Weight", updatedWeight.Kg);
                    command.Parameters.AddWithValue("@Day", updatedWeight.Day);
                    command.Parameters.AddWithValue("@Exercise", updatedWeight.Exercicie.ToString());

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Delete
        public void DeleteWeightById(int weightId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(queryDeleteWeightById, connection))
                {
                    command.Parameters.AddWithValue("@WeightID", weightId);

                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion

        // Método para convertir texto en enum
        private ExerciciesEnum ParseEnum<ExerciciesEnum>(string value)
        {
            return (ExerciciesEnum)Enum.Parse(typeof(ExerciciesEnum), value);
        }
    }
}
