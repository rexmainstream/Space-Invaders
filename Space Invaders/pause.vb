﻿Public Class pause
    Private Sub playlabel_Click(sender As Object, e As EventArgs) Handles playlabel.Click
        'form2 is play
        Form2.tmrenemy.Start()
        Form2.tmrMove.Start()
        Form2.tmrShoot.Start()
        Form2.tmrPowerup.Start()
        Form2.tmrrandomiser.Start()
        Me.Hide()
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        'form2 is play
        Form2.tmrenemy.Start()
        Form2.tmrMove.Start()
        Form2.tmrShoot.Start()
        Form2.tmrPowerup.Start()
        Form2.tmrrandomiser.Start()
        Me.Hide()
    End Sub
    Private Sub pause_KeyPress(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                'form2 is play
                Form2.tmrenemy.Start()
                Form2.tmrMove.Start()
                Form2.tmrShoot.Start()
                Form2.tmrPowerup.Start()
                Form2.tmrrandomiser.Start()
                Me.Hide()
        End Select
    End Sub
    Private Sub SettingsLabel_Click(sender As Object, e As EventArgs) Handles SettingsLabel.Click
        Me.Hide()
        confirmexit.Show()
    End Sub

    Private Sub LevelsLabel_Click(sender As Object, e As EventArgs) Handles LevelsLabel.Click
        Me.Hide()
        levels.Show()
    End Sub

    Private Sub pause_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class