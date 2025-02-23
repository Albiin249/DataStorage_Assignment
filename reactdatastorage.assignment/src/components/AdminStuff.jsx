
import { useNavigate } from "react-router-dom";
const ProjectCreate = () => {
    const navigate = useNavigate();
   

    return (
        <div>
        <div className="container-admin" >
            <div className="create-form">
                <h2>Create</h2>
                    <button className="btn btn-admin" onClick={() => navigate("/CreateUser")}>Create a new user</button>
                    <button className="btn btn-admin" onClick={() => navigate("/CreateCustomer")}>Create a new customer</button>
                    <button className="btn btn-admin" onClick={() => navigate("/CreateService")}>Create a new service</button>
                    <button className="btn btn-admin" onClick={() => navigate("/CreateStatus")}>Create a new statustype</button>
                    
            </div>
                
            <div className="create-form">
                <h2>Remove</h2>
                    <button className="btn btn-admin" onClick={() => navigate("/deleteusers")}>Delete a user</button>
                    <button className="btn btn-admin" onClick={() => navigate("/deletecustomers")}>Delete a customer</button>
                    <button className="btn btn-admin" onClick={() => navigate("/deleteservices")}>Delete a service</button>
                    <button className="btn btn-admin" onClick={() => navigate("/deletestatuses")}>Delete a statustype</button>
                    
            </div>

            </div>
            <button className="btn" onClick={() => navigate("/")}>Back to main page</button>
        </div>
        
    );
};

export default ProjectCreate;
