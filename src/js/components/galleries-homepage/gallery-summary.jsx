import React from 'react';

export default class GallerySummary extends React.Component {

    render(){
        return (
            <li>
                <div>Gallery Name: {this.props.data.name}</div>
                <div>Create Date: {this.props.data.createdDate}</div>
                <div>Description: {this.props.data.description}</div>
                <div>Owner name: {this.props.data.owner.firstName + " " + this.props.data.owner.lastName}</div>
            </li>
        );
    }
}
