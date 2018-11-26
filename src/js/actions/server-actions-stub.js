// stubbed data
let imageId = 0;
const images = 
  [
    {
      id: imageId++,
      url: "./../../src/img/index.png",
      comment: "First Image",
      liked: false,
    },
    {
      id: imageId++,
      url: "./../../src/img/index.png",
      comment: "First Image",
      liked: false,
    },
    {
      id: imageId++,
      url: "./../../src/img/index.png",
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
