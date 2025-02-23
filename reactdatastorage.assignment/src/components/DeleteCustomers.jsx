import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

//Tog inspiration från DeleteUsersPage.
const DeleteCustomers = () => {
    const navigate = useNavigate();
    const [customers, setCustomers] = useState([]);
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        const fetchCustomers = async () => {
            try {
                const res = await fetch("https://localhost:7144/api/project/customers");
                if (res.ok) {
                    const data = await res.json();
                    setCustomers(data);
                } else {
                    setErrorMessage("Failed to load customers.");
                }
            } catch (error) {
                setErrorMessage("Error fetching customers.");
                console.error(error);
            }
        };
        fetchCustomers();
    }, []);

    const handleDelete = async (customerId) => {
        const confirmed = window.confirm("Are you sure you want to delete this customer?"); 
        if (confirmed) {
            try {
                const res = await fetch(`https://localhost:7144/api/project/customers/${customerId}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (res.ok) {
                    
                    setCustomers((prevCustomers) => prevCustomers.filter(customer => customer.id !== customerId));
                } else {
                    const errorData = await res.json();
                    setErrorMessage(errorData.message || "Failed to delete customer.");
                }
            } catch (error) {
                setErrorMessage("Error deleting customer.");
                console.error(error);
            }
        }
    };

    return (
        <div className="users-container">
            <h2 className="project-title">All Customers</h2>
            {errorMessage && <p className="error-message">{errorMessage}</p>}
            <ul>
                {customers.map((customer) => (
                    <div key={customer.id} className="delete-box">
                        <span className="">{customer.customerName}</span>
                        <button className="btn-delete" onClick={() => handleDelete(customer.id)}>Delete</button>

                    </div>
                ))}
            </ul>
            <button className="btn" onClick={() => navigate("/admin")}>Back to admin page</button>
        </div>
    );
};

export default DeleteCustomers;
