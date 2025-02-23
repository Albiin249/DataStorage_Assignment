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
        console.error("Fel vid h�mtning av data:", error);
        }
    }

    useEffect(() => {
        fetchData()
    }, [])

    const handleDelete = async (projectId) => {
        const confirmDelete = window.confirm("Are you sure you want to delete this project?"); //Tog hj�lp av ChatGPT, h�r s� poppas en ruta upp som fr�gar efter bekr�ftelse

        if (confirmDelete) {
            try {
                const res = await fetch(`https://localhost:7144/api/project/${projectId}`, {
                    method: "DELETE",
                });

                if (res.ok) {
                    setProjectItems(prevProjects => prevProjects.filter(project => project.id !== projectId));
                    //Tog hj�lp utav ChatGPT med denna. Filter anv�nds f�r att skapa en ny array d�r endast de objekt som uppfyller villkoren inkluderas, dvs id nummret i detta fallet.
                    //Om id inte �r lika med det specifika project id s� kommer objektet att ing� i den nya arrayen. Den nya arrayen anv�nds sedan, vilket menas d� att projektet med samma ID har tagits bort.
                    alert("Project deleted successfully.");
                } else {
                    console.log("N�got gick fel..");
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

            {projectItems.length === 0 ? ( //Tog hj�lp av ChatGPT h�r, den kikar ifall projectItems inte inneh�ller n�got, om den inte inneh�ller n�got s� skrivs No Projects added ut, annars k�rs map d�r nere och listar ut projekten.
                <p className="no-p">No projects added. Add one!</p>
            ) : (
                <ul className="project-list">
                    {projectItems.map((project, index) => ( //Tog hj�lp av ChatGPT h�r f�r att loopa igenom API datan
                        //Listar ut projekten. Tog lite hj�lp att skapa statuses.find ocks�, men gjort justeringar.
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