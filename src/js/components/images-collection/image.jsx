import React from 'react';
import ImageControl from './image-controls.jsx';
import { ImageDataService } from './data-source';

export default class Image extends React.Component{
    constructor(props){
        super(props);

        this.handledUpdate = this.handledUpdate.bind(this);
        this.handledDelete = this.handledDelete.bind(this);
    }

    handledUpdate(event)
    {
        event.preventDefault();
        console.log('image.handledUpdate not implemented.');
    }

    handledDelete(event){
        event.preventDefault();
        ImageDataService.delteImage(this.props.imageData.id, this.props.userId, data => 
            this.props.onImagesChange());        
    }

    //<Image key={image.blobName} uri={image.uri} comment={image.comment} like={image.liked} imageData={image}/>
    render(){

        return (
            <div>
                <img src={this.props.imageData.uri}></img>
                <div>
                  <div>Comment: {this.props.imageData.comment}</div>
                  <ImageControl context='UPDATE' handledUpdate={this.handledUpdate} handledDelete={this.handledDelete} />  
                </div>
            </div>
        ); 
    }
}