namespace Mentally_project.Services;

public interface IResultsCalculatorService
{
    TestResult CalculateResult(TestDefinition test, Dictionary<int, string> answers);
}

public class ResultsCalculatorService : IResultsCalculatorService
{
    public TestResult CalculateResult(TestDefinition test, Dictionary<int, string> answers)
    {
        var scores = new Dictionary<string, int>();
        
        // Initialize dimensions with zero scores
        foreach (var dimension in test.Results.Dimensions)
        {
            scores[dimension.Key] = 0;
        }
        
        // Calculate scores based on answers
        foreach (var kvp in answers)
        {
            var questionId = kvp.Key;
            var answerId = kvp.Value;
            
            var question = test.Questions.FirstOrDefault(q => q.Id == questionId);
            if (question == null)
                continue;
                
            var answer = question.Answers.FirstOrDefault(a => a.Id == answerId);
            if (answer == null)
                continue;
                
            // Add scores for each dimension
            foreach (var scoreKvp in answer.Score)
            {
                if (scores.ContainsKey(scoreKvp.Key))
                {
                    scores[scoreKvp.Key] += scoreKvp.Value;
                }
            }
        }
        
        // Determine result type based on calculation type
        var resultCode = DetermineResultType(test, scores);
        var resultType = test.Results.Types.FirstOrDefault(t => t.Code == resultCode);
        
        return new TestResult
        {
            TestId = test.Id,
            TestTitle = test.Title,
            CompletedAt = DateTime.Now,
            ResultCode = resultCode ?? "UNKNOWN",
            ResultTitle = resultType?.Title ?? "Неизвестный результат",
            ResultDescription = resultType?.Description ?? "",
            Scores = scores,
            SelectedAnswers = answers.Values.ToList()
        };
    }
    
    private string? DetermineResultType(TestDefinition test, Dictionary<string, int> scores)
    {
        if (test.Results.CalculationType == "dimensional")
        {
            // For dimensional tests like MBTI, compare opposing dimensions
            return CalculateDimensionalResult(test, scores);
        }
        
        // For simple scoring, return the type with highest matching score
        return CalculateSimpleResult(test, scores);
    }
    
    private string? CalculateDimensionalResult(TestDefinition test, Dictionary<string, int> scores)
    {
        // Group dimensions by pairs (E/I, S/N, T/F, J/P for MBTI)
        var dimensions = test.Results.Dimensions;
        var result = "";
        
        // Process pairs of dimensions
        for (int i = 0; i < dimensions.Count - 1; i += 2)
        {
            var dim1 = dimensions[i];
            var dim2 = dimensions[i + 1];
            
            var score1 = scores.GetValueOrDefault(dim1.Key, 0);
            var score2 = scores.GetValueOrDefault(dim2.Key, 0);
            
            result += score1 >= score2 ? dim1.Key : dim2.Key;
        }
        
        // Find matching type
        var matchingType = test.Results.Types.FirstOrDefault(t => t.Code == result);
        return matchingType?.Code ?? result;
    }
    
    private string? CalculateSimpleResult(TestDefinition test, Dictionary<string, int> scores)
    {
        // Find the type with the highest score match
        string? bestMatch = null;
        int bestScore = -1;
        
        foreach (var type in test.Results.Types)
        {
            // Simple matching logic - can be customized per test type
            var typeScore = scores.Values.Sum();
            if (typeScore > bestScore)
            {
                bestScore = typeScore;
                bestMatch = type.Code;
            }
        }
        
        return bestMatch;
    }
}
