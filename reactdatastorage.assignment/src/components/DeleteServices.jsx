import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

//Tog inspiration från DeleteUsersPage.
const DeleteServices = () => {
    const navigate = useNavigate();
    const [products, setProducts] = useState([]);
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const res = await fetch("https://localhost:7144/api/project/services");
                if (res.ok) {
                    const data = await res.json();
                    setProducts(data);
                } else {
                    setErrorMessage("Failed to load services.");
                }
            } catch (error) {
                setErrorMessage("Error fetching services.");
                console.error(error);
            }
        };
        fetchProducts();
    }, []);

    const handleDelete = async (productId) => {
        const confirmed = window.confirm("Are you sure you want to delete this service?");
        if (confirmed) {
            try {
                const res = await fetch(`https://localhost:7144/api/project/services/${productId}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (res.ok) {

                    setProducts((prevProducts) => prevProducts.filter(product => product.id !== productId));
                } else {
                    const errorData = await res.json();
                    setErrorMessage(errorData.message || "Failed to delete service.");
                }
            } catch (error) {
                setErrorMessage("Error deleting service.");
                console.error(error);
            }
        }
    };

    return (
        <div className="users-container">
            <h2 className="project-title">All Services</h2>
            {errorMessage && <p className="error-message">{errorMessage}</p>}
            <ul>
                {products.map((product) => (
                    <div key={product.id} className="delete-box">
                        <span className="">{product.productName}</span>
                        <button className="btn-delete" onClick={() => handleDelete(product.id)}>Delete</button>

                    </div>
                ))}
            </ul>
            <button className="btn" onClick={() => navigate("/admin")}>Back to admin page</button>
        </div>
    );
};

export default DeleteServices;
