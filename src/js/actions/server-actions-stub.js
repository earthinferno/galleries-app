import imagefile from '../../images/index.png';

// stubbed data
let imageId = 0;
const images = 
  [
    {
      id: imageId++,
      url: {imagefile},
      comment: "First Image",
      liked: false,
    },
    {
      id: imageId++,
      url: {imagefile},
      comment: "First Image",
      liked: false,
    },
    {
      id: imageId++,
      url: {imagefile},
      comment: "First Image",
      liked: false,
    }
  ]

export const fetchImages = () => ({
  //return images;
    type: 'FETCH_IMAGES',
    images: images
  });

//export const FETCH_IMAGES = 'FETCH_IMAGES';
