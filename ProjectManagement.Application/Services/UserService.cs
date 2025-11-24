using AutoMapper;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Domain.Models;
using ProjectManagement.Domain.IRepositories;


namespace ProjectManagement.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserContextService _userContext;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, UserContextService userContext, IMapper mapper)
        {
            _userRepository = userRepository;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto?>(user);
        }

        public async Task<UserDto?> GetByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            return _mapper.Map<UserDto?>(user);
        }

        public async Task<User> GetByUserNameForAuthAsync(string userName)
        {
            return await _userRepository.GetByUserNameAsync(userName);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByUserNameAsync(dto.UserName);
            if (existingUser != null)
                throw new BadRequestException("User already exists.");

            var user = _mapper.Map<User>(dto);
            var created = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(created);
        }

        public async Task<UserDto> UpdateAsync(UpdateUserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(dto.Id);
            if (existingUser == null)
                throw new NotFoundException("User not found.");

            var duplicate = await _userRepository.GetByUserNameAsync(dto.UserName);
            if (duplicate != null && duplicate.Id != dto.Id)
                throw new BadRequestException("Another user has this username.");

            _mapper.Map(dto, existingUser);
            var updated = await _userRepository.UpdateAsync(existingUser);
            return _mapper.Map<UserDto>(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            return await _userRepository.DeleteAsync(user);
        }
    }
}