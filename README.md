# ImageSegmentationByKmeans
 Image Segmentation using K-Means algorithm in C#
 
(This project is a university assignment.)
 
 ## Images
<table>
  <tr>
    <td>Input image</td>
    <td>Output image</td>
  </tr>
  <tr>
    <td><img src="https://user-images.githubusercontent.com/67823654/87334828-91ec1080-c53f-11ea-84fd-595d1021a2ab.jpg"></td>
    <td><img src="https://user-images.githubusercontent.com/67823654/87334854-9ca6a580-c53f-11ea-9891-75cf499fa6f5.jpg"></td>
  </tr>
 </table>
 
 ## Image Segmentation Algorithm 
The image segmentation algorithm is very simple and can be formulated as follows:
1. Create an initial cluster containing an original image and a set of centroid pixels randomly selected from the image. Append the initial cluster built to the array of clusters
2. Retrieve the current cluster from the array and iterate through the set of those centroids
3. For each centroid, compute the actual distance to each pixel in the current image. (Euclidian distance)
4. Perform a linear search to find those pixels which value of distance to the current centroid does not exceed a specific boundary
5. Build a new cluster based on the new image containing all those pixels selected at the previous step and the current value of super-pixel. In this case the centroid will serve as a centroid of a newly built cluster. Also, we need to substitute the color of each pixel with the centroid’s color
6. Compute the value of the nearest mean of all centroids in the current cluster by using the center of mass formula to find the coordinates of a particular central point for the set of centroids of the current cluster
7. Perform a check if the coordinates of that central point obtained at the previous step are not equal to the coordinates of the super-pixel in the newly built cluster (the central point has not been moved). If not, append the new cluster to the array of clusters
8. Perform a check if the current cluster is the final cluster in the array of clusters. If so, go to step 9, otherwise return and proceed with step 2
9. Iterate through the array of clusters and merge each particular cluster image into the entire image being segmented
10. Save the segmented image to a file
