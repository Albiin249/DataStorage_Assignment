import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
const ProjectCreate = () => {
    const navigate = useNavigate();
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
    });
    //Tog hj�lp utav ChatGPT f�r att h�mta varje kund, status, produkt och user f�r att ha i dropdown vid create. Kommentarer kommer p� den kod som �r tagen fr�n ChatGPT.

    //H�r skapas states f�r att lagra de h�mtade datalistorna
    const [customers, setCustomers] = useState([]);
    const [statuses, setStatuses] = useState([]);
    const [products, setProducts] = useState([]);
    const [users, setUsers] = useState([]);

    const[submitted, setSubmitted] = useState(false);
    const [errorMessage, setErrorMessage] = useState(""); //State f�r felmeddelanden

    useEffect(() => {
        const fetchData = async () => {
            try {
                //H�r g�rs fetch anrop f�r varje entitet f�r att h�mta data fr�n de olika API l�nkarna.
                //Await Promise.All anv�nds f�r att k�ra flera asynkrona operationer parallelt. V�ntar p� att allt ska bli klart innan man fors�tter med koden.
                const [customersRes, statusesRes, productsRes, usersRes] = await Promise.all([
                    fetch("https://localhost:7144/api/project/customers"),
                    fetch("https://localhost:7144/api/project/status"),
                    fetch("https://localhost:7144/api/project/services"),
                    fetch("https://localhost:7144/api/project/users"),
                ]);

                //H�r h�mtas JSON-data fr�n API responsen.
                const customersData = await customersRes.json();
                const statusesData = await statusesRes.json();
                const productsData = await productsRes.json();
                const usersData = await usersRes.json();

                
                //H�r uppdateras state med den h�mtade datan
                setCustomers(customersData);
                setStatuses(statusesData);
                setProducts(productsData);
                setUsers(usersData);
            } catch (error) {
                console.error("Fel vid h�mtning av data:", error);
            }
        };

        fetchData();
    }, []);  //Tom array g�r att useEffect endast k�rs en g�ng vid laddning

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setProjectData((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMessage(""); //Rensar gamla felmeddelanden

        try {
            const res = await fetch("https://localhost:7144/api/project/projects", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(projectData),
            });

            if (res.ok) {
                setSubmitted(true);
                console.log("Allt lyckades");
            } else {
                const errorData = await res.json();
                setErrorMessage(errorData.message || "N�got gick fel.");
            }
        } catch (error) {
            console.error("Fel vid skapande:", error);
        }
    };

  

    if (submitted) {
        return (
            <div>
                <div>
                    <h3 className="project-title">Project created successfully!</h3>
                    <button className="btn" onClick={() => navigate("/")}>Back to main page</button>
                </div>
            </div>
        );
    }

    return (
        <div className="form-container" >
            <form className="form" onSubmit={handleSubmit}>
                <div className="rubrik">
                    <h2 className="project-title">Create a Project</h2>
                </div>
                <div className="create-form">
                    <input type="text" className="input" name="title" placeholder="Title" value={projectData.title} onChange={handleInputChange} required />
                    <input type="text" className="input" name="description" placeholder="Description" value={projectData.description} onChange={handleInputChange} />
                    <input type="date" className="input" name="startDate" placeholder="Start date, Format: YYYY-MM-DD" value={projectData.startDate} onChange={handleInputChange} required />
                    <input type="date" className="input" name="endDate" placeholder="End date, Format: YYYY-MM-DD" value={projectData.endDate} onChange={handleInputChange} required />
                    

                    {/*Tog hj�lp av ChatGPT f�r att skapa dessa select elementen, f�r att skapa en dropdown meny. */}
                    {/*name anger att de f�lt motsvarar i detta fallet customerId i projectData. */}
                    {/*Onchange handleInputChange, n�r anv�ndaren v�ljer ett alternativ s� k�rs handleinputchange, vilket uppdaterar projectdata.customerId */}
                    {/*customers.map(customer) g�r igenom listan customer och skapar ett <option> f�r varje customer */}
                    <select name="customerId" className="input" value={projectData.customerId} onChange={handleInputChange} required> 
                        <option value="">Select Customer</option>
                        {customers.map((customer) => (
                            <option key={customer.id} value={customer.id}>
                                {customer.customerName}
                            </option>
                        ))}
                    </select> 

                    <select name="statusId" className="input" value={projectData.statusId} onChange={handleInputChange} required>
                        <option value="">Select Status</option>
                        {statuses.map((status) => (
                            <option key={status.id} value={status.id}>
                                {status.statusName}
                            </option>
                        ))}
                    </select>

                    <select name="productId" className="input" value={projectData.productId} onChange={handleInputChange} required>
                        <option value="">Select Product</option>
                        {products.map((product) => (
                            <option key={product.id} value={product.id}>
                                {product.productName}
                            </option>
                        ))}
                    </select>

                    <select name="userId" className="input" value={projectData.userId} onChange={handleInputChange} required>
                        <option value="">Select Project Manager</option>
                        {users.map((user) => (
                            <option key={user.id} value={user.id}>
                                {user.firstName} {user.lastName}
                            </option>
                        ))}
                    </select>

                    {errorMessage && <p className="error-message">{errorMessage}</p>} {/*Skickar ut felmeddelandet*/}
                    <button className="btn btn-create" type="submit">Create Project</button>
                    <button className="btn btn-back" onClick={() => navigate("/")}>Back to main page</button> 
                </div>
            </form>

        </div>
    );
};

export default ProjectCreate;
