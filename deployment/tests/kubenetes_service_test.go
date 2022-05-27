package main

import (
	"context"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
	"testing"
)

func TestEnsureNamespacesExist(t *testing.T) {
	clientset := CreateClientSet()

	namespace, _ := clientset.CoreV1().Namespaces().
		List(context.TODO(), metav1.ListOptions{})

	if len(namespace.Items) < 1 {
		t.Failed()
	}
}
