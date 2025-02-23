import { useState } from "react";
import { useNavigate } from "react-router-dom";
const CreateCustomer = () => {
    const navigate = useNavigate();
    const [customerData, setCustomerData] = useState({
        customerName: "",
    });

    const [submitted, setSubmitted] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCustomerData((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMessage("");
        try {
            const res = await fetch("https://localhost:7144/api/project/customers", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(customerData),

            });
            if (res.ok) {
                setSubmitted(true);
                console.log("Allt lyckades");
            } else {
                const errorData = await res.json();
                setErrorMessage(errorData.message || "Something went wrong while creating the customer.");
            }
        } catch (error) {
            console.error("Fel vid skapande:", error);
        }
    };

    

    if (submitted) {
        return (
            <div>
                <div>
                    <h3 className="project-title">Customer created successfully!</h3>
                    <button className="btn btn-created" onClick={() => navigate("/")}>Back to main page</button>
                    <button className="btn btn-created-back" onClick={() => navigate("/admin")}>Back to admin page</button>
                </div>
            </div>
        );
    }

    return (
        <div className="form-container" >
            <form className="form" onSubmit={handleSubmit}>
                <div className="rubrik">
                    <h2 className="project-title">Create a Customer</h2>
                </div>
                <div className="create-form">
                    <input type="text" className="input" name="customerName" placeholder="Customer Name" value={customerData.customerName} onChange={handleInputChange} required />

                    {errorMessage && <p className="error-message">{errorMessage}</p>} 
                    <button className="btn btn-back" type="submit">Create Customer</button>
                    <button className="btn btn-back" onClick={() => navigate("/")}>Back to main page</button>
                    <button className="btn btn-back" onClick={() => navigate("/admin")}>Back to admin page</button>
                </div>
            </form>

        </div>
    );
};

export default CreateCustomer;
