using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/projects")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public ProjectsController(ApplicationContext context)
    {
        _context = context;
    }

    // ✅ GET all projects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        return await _context.Projects.ToListAsync();
    }

    // ✅ POST a single project
    [HttpPost]
    public async Task<IActionResult> AddProject(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
    }
    [HttpPost("{id}")]
    public async Task<IActionResult>
        UpdateProject(int id, Project project)
    {
        if (id != project.Id)
        {
            return BadRequest();
        }

        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ✅ POST multiple projects (bulk insert)
    [HttpPost("bulk")]
    public async Task<IActionResult> AddMultipleProjects([FromBody] List<Project> projects)
    {
        if (projects == null || projects.Count == 0)
        {
            return BadRequest("Projects list is empty.");
        }

        _context.Projects.AddRange(projects);
        await _context.SaveChangesAsync();
        return Ok(new { message = $"{projects.Count} projects added successfully.", data = projects });
    }
}
