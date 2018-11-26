  class ImageDataService {
    static getImageData() {
      this.imageId = 0;
      this.imageData = 
      [
        {
          id: this.imageId++,
          url: "./../../src/img/index.png",
          comment: "First Image",
          liked: false,
        },
        {
          id: this.imageId++,
          url: "./../../src/img/index.png",
          comment: "First Image",
          liked: false,
        },
        {
          id: this.imageId++,
          url: "./../../src/img/index.png",
          comment: "First Image",
          liked: false,
        }
      ]

      return this.imageData;
    }
  }

  export { ImageDataService }