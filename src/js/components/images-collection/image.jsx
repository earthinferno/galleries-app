import React from 'react';
import ImageControl from './image-controls.jsx'

export default class Image extends React.Component{
    constructor(props){
        super(props);

        this.handledUpdate = this.handledUpdate.bind(this);
        this.handledDelete = this.handledDelete.bind(this);
    }

    handledUpdate(event)
    {
        event.preventDefault();
    }

    handledDelete(event){
        event.preventDefault();
    }

    render(){

        return (
            <div>
                <img src={this.props.uri}></img>
                <div>
                  <div>Comment: {this.props.comment}</div>
                  <ImageControl context='UPDATE' handledUpdate={this.handledUpdate} handledDelete={this.handledDelete} />  
                </div>
            </div>
        ); 
    }
}