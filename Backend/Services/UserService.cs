using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserService(UserRepository userRepository, IMapper mapper, AppDbContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserResponse?> CreateUserAsync(CreateUserDto dto)
        {
            if (await _userRepository.GetUserByUsernameAsync(dto.Username) != null)
                return null;

            if (await _userRepository.GetUserByEmailAsync(dto.Email) != null)
                return null;

            var user = _mapper.Map<User>(dto);
            user.IdUser = await GetNextUserIdAsync();
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

        public async Task<UserResponse?> CreateShipperAsync(CreateShipperRequest dto)
        {
            if (await _userRepository.GetUserByUsernameAsync(dto.Username) != null ||
                await _userRepository.GetUserByEmailAsync(dto.Email) != null)
            {
                return null;
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "shipper");
            if (role == null)
                throw new InvalidOperationException("Khong tim thay vai tro shipper.");

            var user = new User
            {
                IdUser = await GetNextUserIdAsync(),
                IdRole = role.IdRole,
                Username = dto.Username.Trim(),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName?.Trim(),
                Phone = dto.Phone?.Trim(),
                Email = dto.Email.Trim(),
                Address = dto.Address?.Trim(),
                Avatar = string.Empty,
                Status = "active",
                CreatedAt = DateTime.UtcNow,
                CancelRate = 0
            };

            var driver = new Driver
            {
                IdDriver = await _context.Drivers.AnyAsync()
                    ? await _context.Drivers.MaxAsync(d => d.IdDriver) + 1
                    : 1,
                IdUser = user.IdUser,
                LicensePlate = dto.LicensePlate.Trim(),
                Address = dto.Address?.Trim() ?? string.Empty,
                ExpRank = "New",
                DescStatus = "San sang giao hang",
                CurrentLat = 0,
                CurrentLng = 0,
                IsBusy = false,
                RateAvg = 5.0m,
                TotalOrders = 0
            };

            _context.Users.Add(user);
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            user.Role = role;
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            if (!string.IsNullOrWhiteSpace(dto.Email) &&
                await _userRepository.EmailBelongsToAnotherUserAsync(dto.Email.Trim(), id))
            {
                throw new InvalidOperationException("Email này đã được sử dụng bởi tài khoản khác.");
            }

            dto.Email = dto.Email?.Trim();
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

        public async Task<UserResponse?> ToggleStatusAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            user.Status = string.Equals(user.Status, "active", StringComparison.OrdinalIgnoreCase)
                ? "locked"
                : "active";

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            var updatedUser = await _userRepository.GetByIdWithRoleAsync(id);
            return _mapper.Map<UserResponse>(updatedUser ?? user);
        }

        private async Task<int> GetNextUserIdAsync()
        {
            return await _context.Users.AnyAsync()
                ? await _context.Users.MaxAsync(u => u.IdUser) + 1
                : 1;
        }
    }
}

