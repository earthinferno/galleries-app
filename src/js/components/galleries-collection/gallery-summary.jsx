import React from 'react';
import Images from '../images-collection/images-container.jsx';

export default class GallerySummary extends React.Component {
    constructor(props){
        super(props);
        this.state = {
            OpenGalleryExpanded: false,
        };

        this.handleOpenGallery = this.handleOpenGallery.bind(this);

    }

    handleOpenGallery(event){
        //let expanded = !this.state.OpenGalleryExpanded;
        //this.setState({OpenGalleryExpanded: expanded});

        this.props.setGalleryData(this.props.data);
        this.props.history.replace('/home/gallery');


        event.preventDefault();
    }

    render(){
        
        //const expanded = this.state.OpenGalleryExpanded;

        const card = 
            <div className='card'>
                <div className='card-body'>
                    <h5 className='card-title'> {this.props.data.name} </h5>
                    <p className='card-text'> {this.props.data.description} </p>
                    <div className='form-group'>
                        <label >Gallery Owner </label>
                        <input type='text' readOnly className='form-control' name='owner' placeholder={this.props.data.owner.firstName + " " + this.props.data.owner.lastName}/>        
                    </div>
                    <div className='form-group'>
                        <label> Create Date </label>
                        <input type='text' readOnly className='form-control' name='createdDate' placeholder={this.props.data.createdDate}/>        
                    </div>
                    {/* <button className='btn btn-outline-primary' onClick={this.handleOpenGallery}>{expanded ? 'Close' : 'Open Gallery'}</button> */}
                    <button className='btn btn-outline-primary' onClick={this.handleOpenGallery}>Open Gallery</button>
                </div>

            </div>
        ;
        
        return (
            // <div className='row'>
            <div className='col-sm-6'>
                {card}
                {/* <div>
                    {expanded && 
                        <Images galleryData={this.props.data} userId={this.props.userId}/>
                    }
                </div> */}

            </div>

            // </div>

        );

    }
}
