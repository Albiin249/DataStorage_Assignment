import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

//Tog hj�lp utav ChatGPT f�r att skapa denna DeleteUsersPage.
//Har gjort egna justeringar osv.
const DeleteUsersPage = () => {
    const navigate = useNavigate();
    const [users, setUsers] = useState([]);
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        //H�r h�mtas anv�ndare fr�n APIt
        const fetchUsers = async () => {
            try {
                const res = await fetch("https://localhost:7144/api/project/users");
                if (res.ok) {
                    const data = await res.json();
                    setUsers(data);
                } else {
                    setErrorMessage("Failed to load users.");
                }
            } catch (error) {
                setErrorMessage("Error fetching users.");
                console.error(error);
            }
        };
        fetchUsers();
    }, []);

    //H�r hanteras borttagning av anv�ndare
    const handleDelete = async (userId) => {
        const confirmed = window.confirm("Are you sure you want to delete this user?"); //G�r s� att det poppas upp en ruta som fr�gar efter bekr�ftelse.
        if (confirmed) {
            try {
                const res = await fetch(`https://localhost:7144/api/project/users/${userId}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                    },
                });
                if (res.ok) {
                    //Efter borttagningen s� uppdateras anv�ndarlistan
                    setUsers((prevUsers) => prevUsers.filter(user => user.id !== userId));
                } else {
                    const errorData = await res.json();
                    setErrorMessage(errorData.message || "Failed to delete user.");
                }
            } catch (error) {
                setErrorMessage("Error deleting user.");
                console.error(error);
            }
        }
    };

    return (
        <div className="users-container">
            <h2 className="project-title">All Users</h2>
            {errorMessage && <p className="error-message">{errorMessage}</p>}
            <ul>
                {users.map((user) => (
                    <div key={user.id} className="delete-box">
                            <span className="">{user.firstName} {user.lastName} - {user.email}</span>
                            <button className="btn-delete" onClick={() => handleDelete(user.id)}>Delete</button>

                    </div>
                ))}
            </ul>
            <button className="btn" onClick={() => navigate("/admin")}>Back to admin page</button>
        </div>
    );
};

export default DeleteUsersPage;
