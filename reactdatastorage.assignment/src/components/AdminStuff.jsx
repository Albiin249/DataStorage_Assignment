
import { useNavigate } from "react-router-dom";
const ProjectCreate = () => {
    const navigate = useNavigate();
   

    return (
        <div className="container-admin" >
                <div className="create-form">
                    <button className="btn btn-admin" onClick={() => navigate("/CreateUser")}>Create a new user</button>
                    <button className="btn btn-admin" onClick={() => navigate("/CreateCustomer")}>Create a new customer</button>
                    <button className="btn btn-admin" onClick={() => navigate("/CreateService")}>Create a new service</button>
                    <button className="btn btn-admin" onClick={() => navigate("/CreateStatus")}>Create a new statustype</button>
                    <button className="btn btn-admin" onClick={() => navigate("/")}>Back to main page</button>
                </div>
        </div>
    );
};

export default ProjectCreate;
