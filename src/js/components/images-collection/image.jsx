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

    render(){

        const card = 
            <div className='col-sm-6'>
                <div className='card'>
                    <img src={this.props.imageData.uri} className="card-img-top" alt="..."></img>
                    <div className='card-body'>
                        <h5 className='card-title'> Comment </h5>
                        <p className='card-text'>{this.props.imageData.comment} </p>
                        <ImageControl context='UPDATE' handledUpdate={this.handledUpdate} handledDelete={this.handledDelete} />  
                    </div>
                </div>
            </div>;

        return (
            <div>
                {card}
            </div>
        ); 
    }
}