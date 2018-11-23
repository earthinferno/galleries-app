import React from 'react';
import Image from './image.jsx';
import { connect } from 'react-redux';
import { 
  addImage,
} from './../actions';
import { fetchImages } from './../actions/server-actions-stub';

const images = 
  [
    {
        id: 1,
        url: "./../../src/img/index.png",
        comment: "First Image",
        liked: false,
    },
    {
      id: 1,
      url: "./../../src/img/index.png",
      comment: "First Image",
      liked: false,
    },
    {
      id: 1,
      url: "./../../src/img/index.png",
      comment: "First Image",
      liked: false,
    }
  ]

class Images extends React.Component {

  render() {
    const images = this.props.images.images;
    if (images == null)
    {
      return(null);
    }

    return (
        <div>
          {images.map( image => (
            <Image key={image.id} url={image.url} comment={image.comment} like={image.liked}/>
          ))}
        </div>
    );      
  }
}

// const mapDispacthtoProps = (dispatch) => ({
//     image: dispatch(addImage("./../../src/img/index.png","Peace man", true))
//   });
const mapDispacthtoProps = (dispatch) => ({
    images: dispatch(fetchImages())
    }); 

const mapStateToProps = (state) => ({
    images: state.images
  });

export default connect(
    mapStateToProps,
    mapDispacthtoProps
  )(Images);