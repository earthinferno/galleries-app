import React from 'react';
import { RegisterDataService } from './data-source';

export default class Register extends React.Component {
    constructor(props)
    {
        super(props);
        this.state = {
            FirstName: '',
            LastName: '',
            EmailAddress: '',
        };

        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleRegistrationOk = this.handleRegistrationOk.bind(this);

    }
    
    componentDidMount() {
        console.log(this.props.identityProfile);
    }

    handleInputChange()
    {
        const target = event.target;
        const value = target.value;
        const name = target.name;

        this.setState({
          [name]: value
        });
    }

    handleSubmit(event) {
        const account = {
            FirstName: this.state.FirstName,
            LastName: this.state.LastName,
            EmailAddress: this.state.EmailAddress,
            ExternalUserId: this.props.userId,
            ExternalIdentityProvider: this.props.identityProvider , 
        }
        RegisterDataService.registerUser(this.props.userId, account, data =>
            this.handleRegistrationOk(data)
        );
        event.preventDefault();
    }

    handleRegistrationOk(data){
        this.props.history.replace('/home');
    }


    render(){


        const form = 
            <form onSubmit={this.handleSubmit}>
                {/* firstrow */}
                <div className='form-row'>
                
                    {/* Firstname */}
                    <div className='form-group'>
                        <label >First Name</label>
                        <input type='text' className='form-control' name='FirstName' value={this.state.FirstName} onChange={this.handleInputChange} placeholder='Your first name'/>        
                    </div>
                    {/* Lastname */}
                    <div className='form-group'>
                        <label >Last Name</label>
                        <input type='text'  className='form-control' name='LastName' value={this.state.LastName} onChange={this.handleInputChange} placeholder='Your last name'/>        
                    </div>                
                
                
                </div>
                
                {/* Email Address:  */}
                <div className='form-group'>
                    <label >Email Address </label>
                    <input type='text'  className='form-control' name='EmailAddress' value={this.state.EmailAddress} onChange={this.handleInputChange} placeholder='Your email address'/>        
                    <small id='emailaddresshelp' className='form-text test-muted'>We'll never share your email with anyone else.*</small>
                </div>

                <input type="submit" className='btn btn-outline-primary' value="Submit" />
                
            </form>;

        /*
        const firstName = 
                <div className='row justify-content-center'>
                    <div className='col-sm-3'>
                        <label >First Name</label>
                    </div>
                    <div className='col-sm-3'>
                        <input type='text' name='FirstName' value={this.state.FirstName} onChange={this.handleInputChange} placeholder='Your first name'/>        
                    </div>
                </div>;
        
        const lastName = 
                <div className='row justify-content-center'>
                    <div className='col-sm-3'>
                    <label >Last Name</label>
                    </div>
                    <div className='col-sm-3'>
                        <input type='text' name='LastName' value={this.state.LastName} onChange={this.handleInputChange} placeholder='Your last name'/>
                    </div>
                </div>;

        const emailAddress = 
                <div className='row justify-content-center'>
                    <div className='col-sm-3'>
                    <label >Email Address </label>
                    </div>
                    <div className='col-sm-3'>
                        <input type='text' name='EmailAddress' value={this.state.EmailAddress} onChange={this.handleInputChange} placeholder='Your email address'/>        
                        <small id='emailaddresshelp' className='form-text test-muted'>We'll never share your email with anyone else.*</small>
                    </div>
                </div>;

        const submit = 
            <div className='row justify-content-center'>
                <div className='col-sm-6'>
                    <input type="submit" className='btn btn-outline-primary' value="Submit" />
                </div>
            </div>

        const form = 
            <form onSubmit={this.handleSubmit}>            
                {firstName}
                {lastName}
                {emailAddress}
                {submit}
            </form>
*/
        return (
            // <div className='container-flex'>
            //     <div className='row justify-content-around'>
            //         <div className='col-8'>
            //             <div>Welcome: {this.props.userId}</div>
            //         </div>
            //     </div>
            //     {form}
            //     {/* <div className='row justify-content-around'>
            //         <div className=''>
            //             {form}
            //         </div>
            //     </div> */}
            // </div>

            <div className='container-flex'>
                <div className='row justify-content-around'>
                    <div className='col-8'>
                        <div>Welcome: {this.props.userId}</div>
                    </div>
                </div> 
                <div className='row justify-content-around'>
                    <div className='col-8'>
                        {form}
                    </div>
                </div>
            </div>

        );
    }
}
