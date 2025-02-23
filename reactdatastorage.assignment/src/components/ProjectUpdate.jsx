import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";

//Tog hj�lp utav ChatGPT f�r att skapa denna Updatemetoden.

const UpdateProject = () => {
    const { id } = useParams(); //H�r h�mtas projektID fr�n URLen, med hj�lp av useParams
    const navigate = useNavigate(); //H�r skapas en funktion f�r navigering.

    const [projectData, setProjectData] = useState({
        title: "",
        description: "",
        startDate: "",
        endDate: "",
        customerId: "",
        statusId: "",
        userId: "",
        productId: "",
        projectNumber: "",
        totalHours: "",
    });

    const [customers, setCustomers] = useState([]);
    const [statuses, setStatuses] = useState([]);
    const [products, setProducts] = useState([]);
    const [users, setUsers] = useState([]);

    //H�mta befintlig data f�r projektet
    useEffect(() => {
        const fetchProject = async () => {
            try {
                const res = await fetch(`https://localhost:7144/api/project/${id}`);
                const data = await res.json();
                setProjectData(data);
            } catch (error) {
                console.error("Fel vid h�mtning av projekt:", error);
            }
        };

        
        const fetchData = async () => {
            try {
                const [customersRes, statusesRes, productsRes, usersRes] = await Promise.all([
                    fetch("https://localhost:7144/api/project/customers"),
                    fetch("https://localhost:7144/api/project/status"),
                    fetch("https://localhost:7144/api/project/services"),
                    fetch("https://localhost:7144/api/project/users"),
                ]);

                setCustomers(await customersRes.json());
                setStatuses(await statusesRes.json());
                setProducts(await productsRes.json());
                setUsers(await usersRes.json());
            } catch (error) {
                console.error("Fel vid h�mtning av data:", error);
            }
        };

        fetchProject();
        fetchData();
    }, [id]); 

    //H�r hanteras �ndringar i input-f�lt
    const handleInputChange = (e) => { 
        const { name, value } = e.target; //h�r h�mtas namn och value fr�n de som ska uppdateras
        setProjectData((prevState) => ({ //h�r uppdateras states genom att anv�nda den tidigare state och ers�tter endast den egenskap som har �ndrats.
            ...prevState,
            [name]: value, //h�r uppdatares den specfika egenskapen som matchar name med det nya v�rdet.
        }));
    };

    //H�r skickas uppdaterat projekt till API
    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const res = await fetch(`https://localhost:7144/api/project/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(projectData),
            });

            if (res.ok) {
                console.log("Projektet uppdaterades!");
                navigate("/"); // Navigera tillbaka till listan
            } else {
                console.log("N�got gick fel vid uppdatering");
            }
        } catch (error) {
            console.error("Fel vid uppdatering:", error);
        }
    };

    return (
        <div>
            <form className="create-form" onSubmit={handleSubmit}>
                <h2 className="project-title">Edit Project</h2>

                <p className="input-title">Title</p>
                <input className="input" type="text" name="title" value={projectData.title} onChange={handleInputChange} required />
                <p className="input-title">Description</p>
                <input className="input" type="text" name="description" value={projectData.description} onChange={handleInputChange} />
                <p className="input-title">Start Date</p>
                <input className="input" type="date" name="startDate" value={projectData.startDate} onChange={handleInputChange} required />
                <p className="input-title">End Date</p>
                <input className="input" type="date" name="endDate" value={projectData.endDate} onChange={handleInputChange} required />
                <p className="input-title">Project Number</p>
                <input className="input" type="text" name="projectNumber" value={projectData.projectNumber} onChange={handleInputChange} readOnly />

                <p className="input-title">Customer Name</p>
                <select className="input" name="customerId" value={projectData.customerId} onChange={handleInputChange} required>
                    <option value="">Select Customer</option>
                    {/* H�r loopas alla kunder igenom och skapar ett <option> f�r varje kund */}
                    {customers.map((customer) => (
                        <option key={customer.id} value={customer.id}> {/* f�r varje kund s� s�tts kundens id som v�rdet och kundens namn som text */}
                            {customer.customerName}
                        </option>
                    ))}
                </select>

                <p className="input-title">Status</p>
                <select className="input" name="statusId" value={projectData.statusId} onChange={handleInputChange} required>
                    <option value="">Select Status</option>
                    {statuses.map((status) => (
                        <option key={status.id} value={status.id}>
                            {status.statusName}
                        </option>
                    ))}
                </select>

                <p className="input-title">Service</p>
                <select className="input" name="productId" value={projectData.productId} onChange={handleInputChange} required>
                    <option value="">Select Product</option>
                    {products.map((product) => (
                        <option key={product.id} value={product.id}>
                            {product.productName}
                        </option>
                    ))}
                </select>

                <p className="input-title">Project Manager</p>
                <select className="input" name="userId" value={projectData.userId} onChange={handleInputChange} required>
                    <option value="">Select User</option>
                    {users.map((user) => (
                        <option key={user.id} value={user.id}>
                            {user.firstName} {user.lastName}
                        </option>
                    ))}
                </select>
                <p className="input-title">Total Hours</p>
                <input className="input" type="number" name="totalHours" value={projectData.totalHours} onChange={handleInputChange} required />
                <button className="btn btn-back" type="submit">Update Project</button>
                <button onClick={() => navigate("/")} className="btn btn-back">Back to main page</button> 
            </form>
        </div>
    );
};

export default UpdateProject;