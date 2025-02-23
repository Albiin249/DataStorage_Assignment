import { useState } from "react";
import { useNavigate } from "react-router-dom";
const CreateStatus = () => {
    const navigate = useNavigate();
    const [statusData, setStatusData] = useState({
        statusName: "",
    });

    const [submitted, setSubmitted] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setStatusData((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMessage("");
        try {
            const res = await fetch("https://localhost:7144/api/project/status", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(statusData),

            });
            if (res.ok) {
                setSubmitted(true);
                console.log("Allt lyckades");
            } else {
                const errorData = await res.json();
                setErrorMessage(errorData.message || "Något gick fel.");
            }
        } catch (error) {
            console.error("Fel vid skapande:", error);
        }
    };


    if (submitted) {
        return (
            <div>
                <div>
                    <h3 className="project-title">Status created successfully!</h3>
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
                    <h2 className="project-title">Create a Status</h2>
                </div>
                <div className="create-form">
                    <input type="text" className="input" name="statusName" placeholder="Status Name" value={statusData.statusName} onChange={handleInputChange} required />

                    {errorMessage && <p className="error-message">{errorMessage}</p>} 
                    <button className="btn btn-back" type="submit">Create Status</button>
                    <button className="btn btn-back" onClick={() => navigate("/")}>Back to main page</button>
                    <button className="btn btn-back" onClick={() => navigate("/admin")}>Back to admin page</button>
                </div>
            </form>

        </div>
    );
};

export default CreateStatus;
