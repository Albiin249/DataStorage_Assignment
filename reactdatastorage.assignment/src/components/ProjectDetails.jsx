import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";

const ProjectDetails = () => {
    const { id } = useParams(); // Tog inspiration från ProjectUpdate med navigeringen.
    const navigate = useNavigate();

    const [projectData, setProjectData] = useState(null);
    const [customers, setCustomers] = useState([]);
    const [statuses, setStatuses] = useState([]);
    const [products, setProducts] = useState([]);
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const [projectRes, customersRes, statusesRes, productsRes, usersRes] = await Promise.all([
                    fetch(`https://localhost:7144/api/project/${id}`),
                    fetch("https://localhost:7144/api/project/customers"),
                    fetch("https://localhost:7144/api/project/status"),
                    fetch("https://localhost:7144/api/project/services"),
                    fetch("https://localhost:7144/api/project/users"),
                ]);

                setProjectData(await projectRes.json());
                setCustomers(await customersRes.json());
                setStatuses(await statusesRes.json());
                setProducts(await productsRes.json());
                setUsers(await usersRes.json());
            } catch (error) {
                console.error("Fel vid hämtning av data:", error);
            }
        };

        fetchData();
    }, [id]);

    //Tog hjälp från ChatGPT här för att hitta korrekt status/kund/user/service.
    //.find loopar igenom arrayen och returnerar det första objetet som matchar.
    //Fick också hjälp att formatera DateTime snyggt med toLocaleDateString().
    //Navigering med knappen gjordes med hjälp av ChatGPT också, där onclick så navigerar den till sidan /view (tillbaka till projects)
    //Ifall entiteten inte hittas, te.x Customer Name så blir det N/A
    return (
        <div className="create-form">
            <h2 className="project-title">Project Details</h2>
            <p><span className="bold-text">ID:</span> {projectData?.id ?? "N/A"}</p>
            <p><span className="bold-text">Project Number:</span> {projectData?.projectNumber ?? "N/A"}</p>
            <p><span className="bold-text">Title: </span>{projectData?.title ?? "N/A"}</p>
            <p><span className="bold-text">Description: </span>{projectData?.description ?? "N/A"}</p>
            <p><span className="bold-text">Status: </span>{statuses.find(s => s.id === projectData?.statusId)?.statusName ?? "N/A"}</p>
            <p><span className="bold-text">Start Date: </span>{projectData?.startDate ? new Date(projectData.startDate).toLocaleDateString() : "N/A"}</p>
            <p><span className="bold-text">End Date: </span>{projectData?.endDate ? new Date(projectData.endDate).toLocaleDateString() : "N/A"}</p>
            <p><span className="bold-text">Project Manager: </span> {users.find(u => u.id === projectData?.userId)?.firstName} {users.find(u => u.id === projectData?.userId)?.lastName ?? "N/A"}</p>
            <p><span className="bold-text">Customer: </span>{customers.find(c => c.id === projectData?.customerId)?.customerName ?? "N/A"}</p>
            <p><span className="bold-text">Service: </span>{products.find(p => p.id === projectData?.productId)?.productName ?? "N/A"}</p>
            <p><span className="bold-text">TotalPrice: </span>{projectData?.totalPrice ?? "N/A"}</p>

            <button onClick={() => navigate("/")} className="btn">Back to main page</button> 
        </div>
    );
};

export default ProjectDetails;
