using AutoMapper;
using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models.DTO.UserDTOs;
using BookShop.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookShopWeb.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly UserManager<IdentityUser> userManager;        
        private readonly IMapper mapper;
        private readonly IUserStore<IdentityUser> userStore;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUnitOfWork unitOfWork;

        

        public UsersController(
            ILogger<UsersController> logger,
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IUserStore<IdentityUser> userStore,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork
            
            )
        {
            this.logger = logger;
            this.userManager = userManager;            
            this.mapper = mapper;
            this.userStore = userStore;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO )
        { 
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_User_Individual));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company));
            }

            var user = mapper.Map<ApplicationUser>(userDTO);
            user.UserName = userDTO.Email;
            var result = await userManager.CreateAsync(user,
                userDTO.Password
            );
            if (!result.Succeeded)
            {
                return BadRequest(ModelState);
            }

            logger.LogInformation("User created a new account with password");
            if(userDTO.Role == null)
            {
                await userManager.AddToRoleAsync(user, SD.Role_User_Individual);
            }
            else
            {
                await userManager.AddToRoleAsync(user, userDTO.Role);
            }

            user.PasswordHash = null;            
            return Created("", user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login (LoginUserDTO loginData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Email and/or password are not valid");
            }

            var result = await signInManager.PasswordSignInAsync(loginData.Email, loginData.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest(ModelState);
            }

            return Ok(loginData);
        }
    }
}
