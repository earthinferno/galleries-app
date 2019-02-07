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
        console.log("Register:componentDidMount"  + new Date());
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
                    <div className='form-group col-md-6'>
                        <label >First Name</label>
                        <input type='text' className='form-control' name='FirstName' value={this.state.FirstName} onChange={this.handleInputChange} placeholder='Your first name'/>        
                    </div>
                    {/* Lastname */}
                    <div className='form-group col-md-6'>
                        <label >Last Name</label>
                        <input type='text'  className='form-control' name='LastName' value={this.state.LastName} onChange={this.handleInputChange} placeholder='Your last name'/>        
                    </div>                
                </div>
                
                {/* user name */}
                <div className='form-group'>
                    <label >Username </label>
                    <input type='text' readonly className='form-control' name='UserName' value={this.props.userId} placeholder={this.props.userId}/>        
                </div>

                {/* Email Address:  */}
                <div className='form-group'>
                    <label >Email Address </label>
                    <input type='text'  className='form-control' name='EmailAddress' value={this.state.EmailAddress} onChange={this.handleInputChange} placeholder='Your email address'/>        
                    <small id='emailaddresshelp' className='form-text test-muted'>We'll never share your email with anyone else.*</small>
                </div>

                {/* button */}
                <input type="submit" className='btn btn-outline-primary' value="Submit" />
                
            </form>;

        return (
            // center the form
            <div className='container-flex'>
                <div className='row justify-content-around'>
                    <div className='col-8'>
                        {/* welcome message */}
                        <div>Welcome. Please register your details with us</div>
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
