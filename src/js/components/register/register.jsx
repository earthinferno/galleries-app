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
            ExternalIdentityProvider: '',
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
            ExternalIdentityProvider: this.state.ExternalIdentityProvider , 
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

        const form = <form onSubmit={this.handleSubmit}>
                        <div>First Name: <input name='FirstName' type='text' value={this.state.FirstName} onChange={this.handleInputChange} /></div>
                        <div>Last Name: <input name="LastName" type="text" value={this.state.LastName} onChange={this.handleInputChange} /></div>
                        <div>Email Address: <input name="EmailAddress" type="text" value={this.state.EmailAddress} onChange={this.handleInputChange} /></div>
                        <div>External IdentityProvider: <input name="ExternalIdentityProvider" type="text" value={this.state.ExternalIdentityProvider} onChange={this.handleInputChange} /></div>
                        <input type="submit" value="Submit" />
                    </form>;

        return (
            <div>
                <div>Welcome: {this.props.userId}</div>

                {form}
            </div>
        );
    }
}
