using GymProject.Domain;
using GymProject.Repository;

namespace GymProject.Application;

public class WeightManager
{
    private WeightRepository _weightRepository;

    public WeightManager()
    {
        _weightRepository = new WeightRepository();
    }

    public Weight GetWeightById(int weightId)
    {
        return _weightRepository.GetWeightById(weightId);
    }

    public List<Weight> GetLatestWeightsForDifferentExercise(int userId)
    {
        return _weightRepository.GetLatestWeightsForDifferentExercise(userId);
    }

    public List<Weight> GetAverageWeightByExercise(int userId)
    {
        return _weightRepository.GetAverageWeightByExercise(userId);
    }

    public void CreateWeight(Weight newWeight, int userId)
    {
        _weightRepository.InsertWeight(newWeight,userId);
    }

    public void CreateWeights(List<Weight> newWeights,int userId)
    {
        _weightRepository.InsertWeights(newWeights,userId);
    }

    public void UpdateWeightById(int weightId, Weight updatedWeight)
    {
        _weightRepository.UpdateWeightById(weightId, updatedWeight);
    }

    public void DeleteWeightById(int weightId)
    {
        _weightRepository.DeleteWeightById(weightId);
    }
}
