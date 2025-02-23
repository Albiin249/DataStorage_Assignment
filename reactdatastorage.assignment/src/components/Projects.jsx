import { Link, useNavigate } from "react-router-dom";
import { useState, useEffect } from 'react'

const Projects = () => {
    const navigate = useNavigate();

    const [projectItems, setProjectItems] = useState([])
    const [statuses, setStatuses] = useState([]);

    const fetchData = async () => {
        try {
            const [projectRes, statusesRes] = await Promise.all([
                fetch(`https://localhost:7144/api/project/projects`),
                fetch("https://localhost:7144/api/project/status"),
            ]);
        
      
            setProjectItems(await projectRes.json())
            setStatuses(await statusesRes.json());
        
        }
        catch (error) {
        console.error("Fel vid hämtning av data:", error);
        }
    }

    useEffect(() => {
        fetchData()
    }, [])

    const handleDelete = async (projectId) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this project?"); //Tog hjälp av ChatGPT, här så poppas en ruta upp som frågar efter bekräftelse

        if (confirmDelete) {
            try {
                const res = await fetch(`https://localhost:7144/api/project/${projectId}`, {
                    method: "DELETE",
                });

                if (res.ok) {
                    setProjectItems(prevProjects => prevProjects.filter(project => project.id !== projectId));
                    //Tog hjälp utav ChatGPT med denna. Filter används för att skapa en ny array där endast de objekt som uppfyller villkoren inkluderas, dvs id nummret i detta fallet.
                    //Om id inte är lika med det specifika project id så kommer objektet att ingå i den nya arrayen. Den nya arrayen används sedan, vilket menas då att projektet med samma ID har tagits bort.
                    alert("Project deleted successfully.");
                } else {
                    console.log("Något gick fel..");
                }
            } catch (error) {
                console.error("Error:", error);
            }
        }
    };

    return (
        <div className="container"> 
            <Link to="/admin" className="bold-text admin-link">Admin page</Link> 
            <h2 className="project-title">Projects</h2>

            <button onClick={() => navigate("/create")} className="btn">Create a new project</button> 

            {projectItems.length === 0 ? ( //Tog hjälp av ChatGPT här, den kikar ifall projectItems inte innehåller något, om den inte innehåller något så skrivs No Projects added ut, annars körs map där nere och listar ut projekten.
                <p className="no-p">No projects added. Add one!</p>
            ) : (
                <ul className="project-list">
                    {projectItems.map((project, index) => ( //Tog hjälp av ChatGPT här för att loopa igenom API datan
                        //Listar ut projekten. Tog lite hjälp att skapa statuses.find också, men gjort justeringar.
                        <li key={project.id || index}>
                            <p className="project-p"><span className="bold-text">Project number:</span> {project.projectNumber}</p>
                            <p className="project-p"><span className="bold-text">Title:</span> {project.title}</p>
                            <p className="project-p">
                                <span className="bold-text">Status: </span>
                                {
                                    statuses.find(s => s.id === project?.statusId)?.statusName
                                }
                            </p>

                            <Link to={`/edit/${project.id}`}>
                                <button>Edit</button>
                            </Link>
                            <Link to={`/details/${project.id}`}>
                                <button>Details</button>
                            </Link>

                            <button onClick={() => handleDelete(project.id)} >Delete</button>

                        </li>
                    ))}

                </ul>
            )}
        </div>
    );
}

export default Projects