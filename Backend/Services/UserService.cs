using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(UserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponse?> CreateUserAsync(CreateUserDto dto)
        {
            if (await _userRepository.GetUserByUsernameAsync(dto.Username) != null)
                return null;

            if (await _userRepository.GetUserByEmailAsync(dto.Email) != null)
                return null;

            var user = _mapper.Map<User>(dto);
            user.IdRole = 2; // Default role: User
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.Status = "active";
            user.CancelRate = 0;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var createdUser = await _userRepository.GetByIdWithRoleAsync(user.IdUser);
            return _mapper.Map<UserResponse>(createdUser ?? user);
        }

        public async Task<UserResponse?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            _mapper.Map(dto, user);

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            var updatedUser = await _userRepository.GetByIdWithRoleAsync(id);
            return _mapper.Map<UserResponse>(updatedUser ?? user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            return await _userRepository.SaveChangesAsync() > 0;
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            var datas = await _userRepository.GetAllWithRoleAsync();
            return _mapper.Map<List<UserResponse>>(datas);
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdWithRoleAsync(id);
            return user == null ? null : _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return user == null ? null : _mapper.Map<UserResponse>(user);
        }
    }
}

