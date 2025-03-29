using AutoMapper;
using RealEstate_WebAPI.Services;
using RealEstate_WebAPI.Models;
using RealEstate_WebAPI.Repositories.Interfaces;
using RealEstate_WebAPI.Services.Interfaces;
using RealEstate_WebAPI.DTOs.ResponseDTOs;

namespace RealEstate_WebAPI.Services
{
    public class AgentService : BaseService<Agent, AgentResponseDTO>, IAgentService
    {
        private readonly IAgentRepository _agentRepository;

        public AgentService(IAgentRepository agentRepository, IMapper mapper)
            : base(agentRepository, mapper)
        {
            _agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
        }


        public async Task<Agent> GetAgentByUserIdAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return agent;
        }

        public async Task<bool> IsUserAgentAsync(string userId)
        {
            var agent = await _agentRepository.GetByUserIdAsync(userId);
            return agent != null;
        }

        // get agent by property id
        public async Task<AgentResponseDTO> GetAgentByPropertyIdAsync(int propertyId)
        {
            var agent = await _agentRepository.GetByPropertyIdAsync(propertyId);
            return agent;
        }

        // add agent
        public async Task<int> AddAgentAsync(Agent agent)
        {
            return await _agentRepository.AddAsync(agent);
        }

        // UPDATE AGENT
        public async Task UpdateAgentAsync(Agent agent)
        {

            await _agentRepository.UpdateAsync(agent);
        }

        // map agent to agent view model
        public AgentResponseDTO MapAgentToAgentViewModel(Agent agent)
        {
            return new AgentResponseDTO
            {
                FullName = agent.User.FirstName + " " + agent.User.LastName,
                Email = agent.User.Email,
                PhoneNumber = agent.User.PhoneNumber,
                PropertyCount = agent.Properties.Count,
                LicenseNumber = agent.LicenseNumber,
                Agency = agent.Agency,
                Biography = agent.Biography,
                YearsOfExperience = agent.YearsOfExperience,
                ProfileImageUrl = agent.User.UserImageURL
            };
        }

        // map agent view model to agent
        public Agent MapAgentViewModelToAgent(AgentResponseDTO agentViewModel)
        {
            return new Agent
            {
                LicenseNumber = agentViewModel.LicenseNumber,
                Agency = agentViewModel.Agency,
                Biography = agentViewModel.Biography,
                YearsOfExperience = agentViewModel.YearsOfExperience
            };
        }
    }
}