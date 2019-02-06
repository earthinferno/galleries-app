import React from 'react';
import { GalleryDataService } from './data-source';

export default class AddGallery extends React.Component {
    constructor(props)
    {
        super(props);
        this.state = {
            AddGalleryExpanded: false,
            NameValue: '',
            DescriptionValue: '',
        };

        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

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
        //data validation here maybe

        // close modal
        // TODO: replace bootstrap with reactbootstrap
        $('#closeModalButton').click();

        // initialse data object
        const data = {
            Name: this.state.NameValue,
            CreatedDate: new Date(),
            Description: this.state.DescriptionValue,
            userId: this.props.userId,
        }

        GalleryDataService.addGallery(data, galleryData => 
            this.props.onAddGallery(galleryData)
        );        
        event.preventDefault();
    }
    
    render(){

        const modal = 
            <div className="modal fade" id="galleryModal" tabIndex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <form onSubmit={this.handleSubmit}>
                        <div className="modal-header">
                            <h5 className="modal-title" id="exampleModalLabel">Gallery Details</h5>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>             
                        </div>
                        <div className="modal-body">
                            <div className='form-row'>
                                <label>Gallery Name</label>
                                <input name='NameValue' type='text' className='form-control' value={this.state.Namevalue} onChange={this.handleInputChange} />
                            </div>
                            <div className='form-row'>
                                <label>Description</label>
                                <input name="DescriptionValue" type="text" className='form-control' value={this.state.Namevalue} onChange={this.handleInputChange} />
                            </div>
                        </div>
                        <div className="modal-footer">
                            <button type="button" id="closeModalButton" className="btn btn-outline-secondary" data-dismiss="modal">Close</button>
                            <button type="submit" className="btn btn-outline-primary" value="Submit"  >Save changes</button>
                            
                        </div>
                        </form>
                    </div>
                </div>
            </div>;

        return (

            <div className="container">

                <div className='row justify-content-center'>
                    <div className='col-2'>
                        <button type="button" className="btn btn-outline-primary" data-toggle="modal" data-target="#galleryModal">
                            Add Gallery
                        </button>
                        {modal}
                    </div>
                </div>

            </div>
        );
    }
}
