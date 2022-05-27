package main

import (
	"context"
	"fmt"
	"time"

	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
)

func main() {
	clientset := CreateClientSet()

	for {
		pods, err := clientset.CoreV1().Pods("").
			List(context.TODO(), metav1.ListOptions{})

		if err != nil {
			panic(err.Error())
		}
		fmt.Printf("There are %d pods in the cluster\n", len(pods.Items))

		namespacesObj, err := clientset.CoreV1().Namespaces().
			List(context.TODO(), metav1.ListOptions{})

		for _, namespace := range namespacesObj.Items {
			services, _ := clientset.CoreV1().Services(namespace.Namespace).List(context.TODO(), metav1.ListOptions{})

			for _, service := range services.Items {
				print(service.Name)
			}
		}

		time.Sleep(10 * time.Second)
	}
}
