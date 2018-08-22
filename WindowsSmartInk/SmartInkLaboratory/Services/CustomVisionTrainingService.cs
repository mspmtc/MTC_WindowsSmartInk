﻿using Microsoft.Cognitive.CustomVision.Training.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInkLaboratory.Services
{
    public class CustomVisionTrainingService : CustomVisionClassifierBaseService, ITrainingService
    {
        public async Task<IList<Iteration>> GetIterationsAysnc()
        {
            try
            {
                var result = await _trainingApi.GetIterationsWithHttpMessagesAsync(_currentProject.Id);
                return (from i in result.Body select i).OrderByDescending(i => i.TrainedAt).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public  Task<Iteration> TrainAsync()
        {
            return Task.Run(async () => {
                try
                {
                    HttpOperationResponse<Iteration> result;
                    do
                    {
                        result = await _trainingApi.TrainProjectWithHttpMessagesAsync(_currentProject.Id);
                        await Task.Delay(1000);
                    } while (result.Body.Status.ToLower() == "training");
                    return result.Body;
                }
                catch (Exception ex)
                {

                    return null;
                }
            });
        }
    }
}