import React, { useEffect, useRef } from "react";
import * as THREE from "three";
import { GLTFLoader } from "three/examples/jsm/loaders/GLTFLoader";

interface ThreeDViewerProps {
  modelUrl: string;
}

const ThreeDViewer: React.FC<ThreeDViewerProps> = ({ modelUrl }) => {
  const mountRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (mountRef.current) {
      const scene = new THREE.Scene();
      const camera = new THREE.PerspectiveCamera(
        75,
        window.innerWidth / window.innerHeight,
        0.1,
        1000
      );
      const renderer = new THREE.WebGLRenderer();
      renderer.setSize(window.innerWidth, window.innerHeight);
      mountRef.current.appendChild(renderer.domElement);

      const loader = new GLTFLoader();
      loader.load(
        modelUrl,
        (gltf) => {
          scene.add(gltf.scene);
        },
        (xhr) => {
          console.log((xhr.loaded / xhr.total) * 100 + "% loaded");
        },
        (error) => {
          console.error("An error happened", error);
        }
      );

      const light = new THREE.AmbientLight(0xffffff, 1);
      scene.add(light);

      camera.position.z = 5;

      const animate = () => {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
      };

      animate();

      const handleResize = () => {
        const width = window.innerWidth;
        const height = window.innerHeight;
        renderer.setSize(width, height);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
      };

      window.addEventListener("resize", handleResize);

      return () => {
        window.removeEventListener("resize", handleResize);
        mountRef.current?.removeChild(renderer.domElement);
      };
    }
  }, [modelUrl]);

  return <div ref={mountRef} style={{ width: "100%", height: "400px" }} />;
};

export default ThreeDViewer;
