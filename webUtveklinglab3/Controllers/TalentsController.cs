using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/talents")]
public class TalentsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public TalentsController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
    {
        return await _context.Skills.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateSkill(Skill skill)
    {
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSkills), new { id = skill.Id }, skill);
    }
}
