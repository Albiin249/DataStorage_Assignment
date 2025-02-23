import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

//Tog inspiration från DeleteUsersPage.
const DeleteStatus = () => {
    const navigate = useNavigate();
    const [statuses, setStatus] = useState([]);
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        const fetchStatuses = async () => {
            try {
                const res = await fetch("https://localhost:7144/api/project/status");
                if (res.ok) {
                    const data = await res.json();
                    setStatus(data);
                } else {
                    setErrorMessage("Failed to load statuses.");
                }
            } catch (error) {
                setErrorMessage("Error fetching statuses.");
                console.error(error);
            }
        };
        fetchStatuses();
    }, []);

    const handleDelete = async (statusId) => {
        const confirmed = window.confirm("Are you sure you want to delete this status?"); 
        if (confirmed) {
            try {
                const res = await fetch(`https://localhost:7144/api/project/status/${statusId}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (res.ok) {

                    setStatus((prevStatus) => prevStatus.filter(status => status.id !== statusId));
                } else {
                    const errorData = await res.json();
                    setErrorMessage(errorData.message || "Failed to delete status.");
                }
            } catch (error) {
                setErrorMessage("Error deleting status.");
                console.error(error);
            }
        }
    };

    return (
        <div className="users-container">
            <h2 className="project-title">All Statustypes</h2>
            {errorMessage && <p className="error-message">{errorMessage}</p>}
            <ul>
                {statuses.map((status) => (
                    <div key={status.id} className="delete-box">
                        <span className="">{status.statusName}</span>
                        <button className="btn-delete" onClick={() => handleDelete(status.id)}>Delete</button>

                    </div>
                ))}
            </ul>
            <button className="btn" onClick={() => navigate("/admin")}>Back to admin page</button>
        </div>
    );
};

export default DeleteStatus;
